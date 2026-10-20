using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest.DTOs;
using NewProject.Infrastructure.Persistence;
using NewProject.Application.Common.CurrentUser;
using NewProject.Common.Results;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Security.Claims;
using MultipleFormat.Application.Modules.Supervisors;

namespace NewProject.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/changes-department-or-shift")]
    public class DepartmentShiftChangeRequestController : ControllerBase
    {
        private readonly ChangesDepOrShiftRequestService _changesDepOrShiftRequestService;
        private readonly ICurrentUserService _currentUserService;
        private readonly SupervisorService _supervisorService;

        public DepartmentShiftChangeRequestController(
            ChangesDepOrShiftRequestService changesDepOrShiftRequestService,
            ICurrentUserService currentUserService,
            SupervisorService supervisorService)
        {
            _changesDepOrShiftRequestService = changesDepOrShiftRequestService;
            _currentUserService = currentUserService;
            _supervisorService = supervisorService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChangesDepOrShiftCreateRequestDTO dto)
        {
            var currentUserResult = await _currentUserService.GetCurrentEmployeeAsync(User, HttpContext.RequestAborted);
            if (!currentUserResult.IsSuccess || currentUserResult.Data == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = currentUserResult.Message ?? "El usuario no tiene acceso habilitado a la aplicación." });
            }

            var currentUser = currentUserResult.Data;
            var isAdministrator = await _supervisorService.IsAdministratorAsync(
                User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name,
                HttpContext.RequestAborted);

            var isAuthorizedSupervisor = currentUser.SupervisorId.HasValue &&
                await _changesDepOrShiftRequestService.IsEmployeeUnderSupervisorAsync(
                    currentUser.SupervisorId.Value,
                    dto.EmployeeId,
                    HttpContext.RequestAborted);

            if (!isAdministrator && !isAuthorizedSupervisor)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = "El empleado indicado no pertenece al ámbito del usuario autenticado."
                });
            }

            var result = await _changesDepOrShiftRequestService.CreateChangesDepOrShiftRequests(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("supervised-employees")]
        public async Task<IActionResult> GetSupervisedEmployees(CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserService.GetCurrentEmployeeAsync(User, cancellationToken);
            if (!currentUserResult.IsSuccess || currentUserResult.Data == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = currentUserResult.Message ?? "El usuario no tiene acceso habilitado a la aplicación." });
            }

            var supervisorId = currentUserResult.Data.SupervisorId;
            if (!supervisorId.HasValue)
            {
                return Ok(ServiceResult<List<SupervisedEmployeeDto>>.Success(new List<SupervisedEmployeeDto>()));
            }

            var result = await _changesDepOrShiftRequestService.GetSupervisedEmployeesAsync(supervisorId.Value, cancellationToken);
            return Ok(result);
        }

        [HttpGet("get-changes-data")]
        public async Task<IActionResult> GetChangesData(CancellationToken cancellationToken)
        {
            ServiceResult<CurrentUserDto> currentUserResult = await _currentUserService.GetCurrentEmployeeAsync(User, cancellationToken);

            if (!currentUserResult.IsSuccess || currentUserResult.Data == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = currentUserResult.Message ?? "El usuario no tiene acceso habilitado a la aplicación." });
            }

            int employeeId = currentUserResult.Data.Id!;

            var result = await _changesDepOrShiftRequestService.GetChangesDepOrShiftRequestByEmployeeAsync(employeeId, cancellationToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
