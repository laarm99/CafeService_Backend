using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultipleFormat.Application.Modules.Supervisors;
using MultipleFormat.Application.Modules.Supervisors.DTOs;
using System.Security.Claims;

namespace NewProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/supervisors")]
    public class SupervisorController : ControllerBase
    {
        private readonly SupervisorService _supervisorService;

        public SupervisorController(SupervisorService supervisorService)
        {
            _supervisorService = supervisorService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(bool includeInactive = true, CancellationToken cancellationToken = default)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var result = await _supervisorService.GetAsync(includeInactive, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var result = await _supervisorService.GetByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateSupervisorRequest request,
            CancellationToken cancellationToken)
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

        [HttpPost("sync-from-employees")]
        public async Task<IActionResult> SyncFromEmployees(CancellationToken cancellationToken)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var result = await _supervisorService.SyncFromEmployeesAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPut("employees/{employeeId:int}")]
        public async Task<IActionResult> AssignEmployee(
            int employeeId,
            AssignEmployeeSupervisorRequest request,
            CancellationToken cancellationToken)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var result = await _supervisorService.AssignEmployeeAsync(
                employeeId,
                request.SupervisorId,
                cancellationToken);

            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        private Task<bool> IsAdministratorAsync(CancellationToken cancellationToken)
        {
            var identityName = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name;
            return _supervisorService.IsAdministratorAsync(identityName, cancellationToken);
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
