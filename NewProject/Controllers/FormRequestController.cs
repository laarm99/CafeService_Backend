using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultipleFormat.Application.Modules.FormRequests;
using MultipleFormat.Application.Modules.FormRequests.DTOs;
using NewProject.Application.Common.CurrentUser;
using System.Security.Claims;

namespace NewProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/form-requests")]
    public class FormRequestController : ControllerBase
    {
        private readonly FormRequestService _formRequestService;
        private readonly ICurrentUserService _currentUserService;
        private readonly FormRequestApprovalService _approvalService;

        public FormRequestController(
            FormRequestService formRequestService,
            ICurrentUserService currentUserService,
            FormRequestApprovalService approvalService)
        {
            _formRequestService = formRequestService;
            _currentUserService = currentUserService;
            _approvalService = approvalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetVisible(
            [FromQuery] FormRequestFilterDto? filter,
            CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserService
                .GetCurrentEmployeeAsync(User, cancellationToken);

            if (!currentUserResult.IsSuccess || currentUserResult.Data?.AccountId is not int)
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
                .GetVisibleAsync(currentUserResult.Data, filter, cancellationToken);

            if (!result.IsSuccess)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new { message = result.Message ?? "El usuario no tiene un rol habilitado." });
            }

            return Ok(result);
        }

        [HttpPost("{id:int}/approve")]
        public async Task<IActionResult> Approve(
            int id,
            FormRequestDecisionDto decision,
            CancellationToken cancellationToken)
        {
            var result = await _approvalService.ApproveAsync(
                id,
                User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name,
                decision,
                cancellationToken);

            return result.IsSuccess
                ? Ok(result)
                : StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("{id:int}/reject")]
        public async Task<IActionResult> Reject(
            int id,
            FormRequestDecisionDto decision,
            CancellationToken cancellationToken)
        {
            var result = await _approvalService.RejectAsync(
                id,
                User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name,
                decision,
                cancellationToken);

            return result.IsSuccess
                ? Ok(result)
                : StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDetail(int id, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserService
                .GetCurrentEmployeeAsync(User, cancellationToken);

            if (!currentUserResult.IsSuccess || currentUserResult.Data?.AccountId is not int)
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
