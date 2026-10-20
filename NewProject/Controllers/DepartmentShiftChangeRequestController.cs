using Microsoft.AspNetCore.Mvc;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest.DTOs;
using MultipleFormat.Application.Modules.PermissionsRequest;
using NewProject.Application.Common.CurrentUser;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;

namespace NewProject.Controllers
{
    [ApiController]
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
            var currentUserResult = await _currentUserService.GetCurrentEmployeeAsync(User, HttpContext.RequestAborted);
            if (!currentUserResult.IsSuccess || currentUserResult.Data?.AccountId is not int createdById)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "La cuenta del usuario no está configurada." });
            }

            var result = await _changesDepOrShiftRequestService.CreateChangesDepOrShiftRequests(dto, createdById);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
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
