using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultipleFormat.Application.Modules.FormRequests;
using NewProject.Application.Common.CurrentUser;

namespace NewProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/form-requests")]
    public class FormRequestController : ControllerBase
    {
        private readonly FormRequestService _formRequestService;
        private readonly ICurrentUserService _currentUserService;

        public FormRequestController(
            FormRequestService formRequestService,
            ICurrentUserService currentUserService)
        {
            _formRequestService = formRequestService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetVisible(CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserService
                .GetCurrentEmployeeAsync(User, cancellationToken);

            if (!currentUserResult.IsSuccess || currentUserResult.Data == null)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new
                    {
                        message = currentUserResult.Message ??
                                  "El usuario no tiene acceso habilitado a la aplicación."
                    });
            }

            var result = await _formRequestService
                .GetVisibleAsync(currentUserResult.Data, cancellationToken);

            if (!result.IsSuccess)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new { message = result.Message ?? "El usuario no tiene un rol habilitado." });
            }

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDetail(int id, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserService
                .GetCurrentEmployeeAsync(User, cancellationToken);

            if (!currentUserResult.IsSuccess || currentUserResult.Data == null)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new
                    {
                        message = currentUserResult.Message ??
                                  "El usuario no tiene acceso habilitado a la aplicación."
                    });
            }

            var result = await _formRequestService.GetVisibleDetailAsync(
                id,
                currentUserResult.Data,
                cancellationToken);

            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
