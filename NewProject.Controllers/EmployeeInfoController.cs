using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MultipleFormat.Application.Modules.EmployeeInfo;
using MultipleFormat.Application.Modules.EmployeeInfo.DTOs;
using NewProject.Application.Common.CurrentUser;
using NewProject.Common.Results;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MultipleFormat.Web.Controllers
{
    [ApiController]
    [Route("api/employee-info")]
    public class EmployeeInfoController : ControllerBase
    {
        private readonly EmployeeInfoService _employeeInfoService;
        private readonly ICurrentUserService _currentUserService;

        public EmployeeInfoController(EmployeeInfoService employeeInfoService, ICurrentUserService currentUserService)
        {
            _employeeInfoService = employeeInfoService;
            _currentUserService = currentUserService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentEmployeeInfo(CancellationToken cancellationToken)
        {
            ServiceResult<CurrentUserDto> currentUserResult = await _currentUserService.GetCurrentEmployeeAsync(User, cancellationToken);

            if (!currentUserResult.IsSuccess || currentUserResult.Data == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = currentUserResult.Message ?? "El usuario no tiene acceso habilitado a la aplicación." });
            }

            int employeeId = currentUserResult.Data.Id!;

            ServiceResult<EmployeeInfoDTO> result = await _employeeInfoService.GetEmployeeInfoByEmployeeIdAsync(employeeId, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
