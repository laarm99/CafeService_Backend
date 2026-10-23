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
        private readonly FormRequestAdminService _adminService;
        private readonly NewProject.Application.Common.Storage.AttachmentStorage _attachmentStorage;

        public FormRequestController(
            FormRequestService formRequestService,
            ICurrentUserService currentUserService,
            FormRequestApprovalService approvalService,
            FormRequestAdminService adminService,
            NewProject.Application.Common.Storage.AttachmentStorage attachmentStorage)
        {
            _attachmentStorage = attachmentStorage;
            _formRequestService = formRequestService;
            _currentUserService = currentUserService;
            _approvalService = approvalService;
            _adminService = adminService;
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

        [HttpPost("{id:int}/complete")]
        public async Task<IActionResult> Complete(int id, CancellationToken cancellationToken)
        {
            var result = await _adminService.CompleteAsync(
                id,
                User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name,
                cancellationToken);

            return result.IsSuccess
                ? Ok(result)
                : StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserService.GetCurrentEmployeeAsync(User, cancellationToken);
            if (!currentUserResult.IsSuccess || currentUserResult.Data?.AccountId is not int)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "El usuario no tiene acceso habilitado a la aplicación." });
            }

            var result = await _formRequestService.CancelAsync(id, currentUserResult.Data, cancellationToken);
            return result.IsSuccess
                ? Ok(result)
                : StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id:int}/pdf")]
        public async Task<IActionResult> DownloadPdf(int id, CancellationToken cancellationToken)
        {
            var result = await _adminService.GetPdfAsync(
                id,
                User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name,
                cancellationToken);

            return result.IsSuccess && result.Data != null
                ? File(result.Data, "application/pdf", $"formato-multiple-{id}.pdf")
                : StatusCode((int)result.StatusCode, new { message = result.Message });
        }

        [HttpGet("{id:int}/attachments/{fileId}")]
        public async Task<IActionResult> DownloadAttachment(int id, string fileId, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserService
                .GetCurrentEmployeeAsync(User, cancellationToken);

            if (!currentUserResult.IsSuccess || currentUserResult.Data?.AccountId is not int)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "El usuario no tiene acceso habilitado a la aplicación." });
            }

            // El detalle ya aplica las reglas de visibilidad por rol.
            var detail = await _formRequestService.GetVisibleDetailAsync(id, currentUserResult.Data, cancellationToken);
            if (!detail.IsSuccess || detail.Data == null || detail.Data.Attachments.All(x => x.Id != fileId))
            {
                return NotFound(new { message = "Archivo no encontrado." });
            }

            var file = _attachmentStorage.Open(id, fileId);
            if (file == null)
            {
                return NotFound(new { message = "Archivo no encontrado." });
            }

            new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider()
                .TryGetContentType(file.Value.Name, out var contentType);

            return File(file.Value.Stream, contentType ?? "application/octet-stream", file.Value.Name);
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
