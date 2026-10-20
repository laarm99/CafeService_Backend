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
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            if (!await IsAdministratorAsync(cancellationToken))
            {
                return Forbid();
            }

            var result = await _supervisorService.GetActiveAsync(cancellationToken);
            return Ok(result);
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
    }
}
