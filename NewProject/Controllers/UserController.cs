using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewProject.Application.Modules.Users;
using NewProject.Application.Modules.Users.DTOs;
using System.Security.Claims;

namespace NewProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            bool includeInactive = true,
            CancellationToken cancellationToken = default)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            return Ok(await _userService.GetAsync(includeInactive, cancellationToken));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var result = await _userService.GetByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateUserRequest request,
            CancellationToken cancellationToken)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var currentUserId = GetCurrentAccountId();
            var result = await _userService.UpdateAsync(id, request, currentUserId, cancellationToken);
            return result.IsSuccess
                ? Ok(result)
                : StatusCode((int)result.StatusCode, result);
        }

        private Task<bool> IsAdministratorAsync(CancellationToken cancellationToken)
        {
            var identityName = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name;
            return _userService.IsAdministratorAsync(identityName, cancellationToken);
        }

        private int? GetCurrentAccountId()
        {
            var accountIdClaim = User.FindFirstValue("AccountId");
            if (int.TryParse(accountIdClaim, out var accountId))
            {
                return accountId;
            }
            return null;
        }
    }
}
