using Microsoft.AspNetCore.Mvc;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest.DTOs;
using NewProject.Infrastructure.Persistence;
using NewProject.Application.Common.CurrentUser;
using NewProject.Common.Results;
using Microsoft.AspNetCore.Http;
using System.Threading;

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

        [HttpPost]
        public async Task<IActionResult> Create(ChangesDepOrShiftCreateRequestDTO dto)
        {
            var result = await _changesDepOrShiftRequestService.CreateChangesDepOrShiftRequests(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

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
