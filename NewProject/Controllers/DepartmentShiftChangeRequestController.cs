using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest.DTOs;
using MultipleFormat.Application.Modules.PermissionsRequest;
using NewProject.Application.Common.CurrentUser;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;

namespace NewProject.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/changes-department-or-shift")]
    public class DepartmentShiftChangeRequestController : ControllerBase
    {
        private readonly ChangesDepOrShiftRequestService _changesDepOrShiftRequestService;
        private readonly ICurrentUserService _currentUserService;

        public DepartmentShiftChangeRequestController(ChangesDepOrShiftRequestService changesDepOrShiftRequestService, ICurrentUserService currentUserService)
        {
            _changesDepOrShiftRequestService = changesDepOrShiftRequestService;
            _currentUserService = currentUserService;
        }

        [HttpPost("create-change-request")]
        public async Task<IActionResult> CreateChangeRequest(ChangesDepOrShiftCreateRequestDTO dto)
        {
            // Deshabilitado temporalmente: el cliente evalua si se usara este formato.
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "La solicitud de cambio de departamento/turno no está habilitada." });
#pragma warning disable CS0162
            var currentUserResult = await _currentUserService.GetCurrentEmployeeAsync(User, HttpContext.RequestAborted);
            if (!currentUserResult.IsSuccess || currentUserResult.Data is not { AccountId: int createdById } currentUser)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "La cuenta del usuario no está configurada." });
            }

            if (dto.EmployeeId != currentUser.Id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Solo puede crear solicitudes para sí mismo." });
            }

            var result = await _changesDepOrShiftRequestService.CreateChangesDepOrShiftRequests(dto, createdById);

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
                return StatusCode(StatusCodes.Status403Forbidden, new { message = currentUserResult.Message ?? "El usuario no tiene acceso habilitado a la aplicaciÃ³n." });
            }

            var supervisorId = currentUserResult.Data.SupervisorId;
            if (!supervisorId.HasValue)
            {
                return Ok(ServiceResult<List<SupervisedEmployeeDto>>.Success(new List<SupervisedEmployeeDto>()));
            }

            var result = await _changesDepOrShiftRequestService.GetSupervisedEmployeesAsync(supervisorId.Value, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-change-request-data")]
        public async Task<IActionResult> GetChangeRequestData(CancellationToken cancellationToken)
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
