using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultipleFormat.Application.Modules.Supervisors;
using MultipleFormat.Application.Modules.Supervisors.DTOs;
using NewProject.Application.Modules.Users;
using NewProject.Application.Modules.Users.DTOs;
using System.Security.Claims;

namespace NewProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/configuration")]
    public class ConfigurationController : ControllerBase
    {
        private readonly SupervisorService _supervisorService;
        private readonly UserService _userService;

        public ConfigurationController(
            SupervisorService supervisorService,
            UserService userService)
        {
            _supervisorService = supervisorService;
            _userService = userService;
        }

        #region Supervisores

        [HttpGet("supervisors")]
        public async Task<IActionResult> GetSupervisors(
            bool includeInactive = true,
            CancellationToken cancellationToken = default)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var result = await _supervisorService.GetAsync(includeInactive, cancellationToken);
            return Ok(result);
        }

        [HttpGet("supervisors/{id:int}")]
        public async Task<IActionResult> GetSupervisorById(
            int id,
            CancellationToken cancellationToken = default)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var result = await _supervisorService.GetByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("supervisors/{id:int}")]
        public async Task<IActionResult> UpdateSupervisor(
            int id,
            UpdateSupervisorRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var currentUserId = GetCurrentAccountId();
            var result = await _supervisorService.UpdateAsync(id, request, currentUserId, cancellationToken);
            return result.IsSuccess
                ? Ok(result)
                : StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("supervisors/{id:int}/status")]
        public async Task<IActionResult> ToggleSupervisorStatus(
            int id,
            UpdateSupervisorStatusRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var currentUserId = GetCurrentAccountId();
            var result = await _supervisorService.ToggleActiveAsync(id, request.IsActive, currentUserId, cancellationToken);
            return result.IsSuccess
                ? Ok(result)
                : StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Usuarios

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(
            bool includeInactive = true,
            CancellationToken cancellationToken = default)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var result = await _userService.GetAsync(includeInactive, cancellationToken);
            return Ok(result);
        }

        [HttpGet("users/{id:int}")]
        public async Task<IActionResult> GetUserById(
            int id,
            CancellationToken cancellationToken = default)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var result = await _userService.GetByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("users/{id:int}/role")]
        public async Task<IActionResult> UpdateUserRole(
            int id,
            UpdateUserRequest request,
            CancellationToken cancellationToken = default)
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

        [HttpPut("users/{id:int}/roles")]
        public async Task<IActionResult> UpdateUserRoles(
            int id,
            UpdateUserRolesRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var currentUserId = GetCurrentAccountId();
            var result = await _userService.UpdateRolesAsync(id, request, currentUserId, cancellationToken);
            return result.IsSuccess
                ? Ok(result)
                : StatusCode((int)result.StatusCode, result);
        }

        [HttpPatch("users/{id:int}/status")]
        public async Task<IActionResult> ToggleUserStatus(
            int id,
            UpdateUserStatusRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var currentUserId = GetCurrentAccountId();
            var result = await _userService.ToggleActiveAsync(id, request.IsActive, currentUserId, cancellationToken);
            return result.IsSuccess
                ? Ok(result)
                : StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Helpers

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

        #endregion
    }
}
