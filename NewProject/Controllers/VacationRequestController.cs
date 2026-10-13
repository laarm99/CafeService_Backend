using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MultipleFormat.Application.Modules.VacationsRequest;
using MultipleFormat.Application.Modules.VacationsRequest.DTOs;
using NewProject.Application.Common.CurrentUser;
using NewProject.Common.Results;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultipleFormat.Web.Controllers
{
    [ApiController]
    [Route("api/vacation-request")]
    public class VacationRequestController : ControllerBase
    {
        private readonly VacationRequestService _vacationRequestService;
        private readonly ICurrentUserService _currentUserService;

        public VacationRequestController(VacationRequestService vacationRequestService, ICurrentUserService currentUserService)
        {
            _vacationRequestService = vacationRequestService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(VacationCreateRequestDTO dto)
        {
            ServiceResult<CurrentUserDto> currentUserResult = await _currentUserService.GetCurrentEmployeeAsync(User, HttpContext.RequestAborted);

            if (!currentUserResult.IsSuccess || currentUserResult.Data == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = currentUserResult.Message ?? "El usuario no tiene acceso habilitado a la aplicación." });
            }

            CurrentUserDto employee = currentUserResult.Data!;

            ServiceResult<int> result = await _vacationRequestService.CreateVacationRequest(dto, employee.Id);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("get-vacation-data")]
        public async Task<IActionResult> GetVacationData(CancellationToken cancellationToken)
        {
            ServiceResult<CurrentUserDto> currentUserResult = await _currentUserService.GetCurrentEmployeeAsync(User, cancellationToken);

            if (!currentUserResult.IsSuccess || currentUserResult.Data == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = currentUserResult.Message ?? "El usuario no tiene acceso habilitado a la aplicación." });
            }

            CurrentUserDto employee = currentUserResult.Data!;

            ServiceResult<List<VacationRequestDTO>> result = await _vacationRequestService.GetVacationRequestsByEmployeeAsync(employee.Id, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
