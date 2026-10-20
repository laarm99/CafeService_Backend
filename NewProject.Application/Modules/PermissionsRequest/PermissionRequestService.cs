
using Microsoft.EntityFrameworkCore;
using MultipleFormat.Application.Modules.PermissionsRequest.DTOs;
using MultipleFormat.Application.Modules.FormRequests;
using MultipleFormat.Application.Modules.VacationsRequest.DTOs;
using MultipleFormat.Domain.Common.Enums;
using MultipleFormat.Domain.Entities.FlowRequests;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;

namespace MultipleFormat.Application.Modules.PermissionsRequest
{
    public class PermissionRequestService
    {
        private readonly MultipleFormatDbContext _ctx;
        private readonly FormRequestApprovalService _approvalService;

        public PermissionRequestService(MultipleFormatDbContext ctx, FormRequestApprovalService approvalService)
        {
            _ctx = ctx;
            _approvalService = approvalService;
        }

        public async Task<ServiceResult<int>> CreatePermissionRequest(PermissionCreateRequestDTO dto, int createdById, CancellationToken cancellationToken = default)
        {
            // Validate attachments for types that require comprobantes (Marriage, Birth, Death)
            if ((dto.PermissionType == PermissionType.Marriage || dto.PermissionType == PermissionType.Birth || dto.PermissionType == PermissionType.Death)
                && (dto.Attachments == null || dto.Attachments.Count == 0))
            {
                return ServiceResult<int>.BadRequest("Adjuntos comprobatorios requeridos para el tipo de permiso seleccionado.");
            }

            FormRequest formRequestData = new()
            {
                EmployeeId = dto.EmployeeId,
                CreatedById = createdById,
                ApproverId = await _approvalService.ResolveApproverIdAsync(dto.EmployeeId, cancellationToken),
                FormType = FormType.Permission,
                Status = RequestStatus.Submitted,
                CreatedDate = DateTime.UtcNow,
                Comments = dto.Comments
            };

            _ctx.FormRequests.Add(formRequestData);

            await _ctx.SaveChangesAsync(cancellationToken);


            PermissionRequest permissionRequestData = new()
            {
                FormRequestId = formRequestData.Id,
                PermissionType = dto.PermissionType,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate ?? dto.StartDate,
                ReturnDate = dto.ReturnDate ?? dto.StartDate,
                PermissionDays = dto.PermissionDays,
                SwapDetail = dto.SwapDetail
            };

            _ctx.PermissionRequests.Add(permissionRequestData);

            await _ctx.SaveChangesAsync(cancellationToken);

            // Note: file saving for Attachments is not implemented here. Save files to storage and persist references in a separate table.

            return ServiceResult<int>.Success(formRequestData.Id);
        }

        public async Task<ServiceResult<List<VacationRequestDTO>>> GetPermissionRequestByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            List<VacationRequestDTO> requests = await _ctx.FormRequests
                .Where(fr => fr.EmployeeId == employeeId && fr.FormType == FormType.Permission)
                .Select(fr => new VacationRequestDTO
                {
                    FormRequestId = fr.Id,
                    FormType = fr.FormType,
                    Status = fr.Status,
                    CreatedDate = fr.CreatedDate,
                    SubmittedDate = fr.SubmittedDate,
                    Comments = fr.Comments,
                    StartDate = _ctx.PermissionRequests.Where(pr => pr.FormRequestId == fr.Id).Select(pr => pr.StartDate).FirstOrDefault(),
                    EndDate = _ctx.PermissionRequests.Where(pr => pr.FormRequestId == fr.Id).Select(pr => pr.EndDate).FirstOrDefault(),
                    ReturnDate = _ctx.PermissionRequests.Where(pr => pr.FormRequestId == fr.Id).Select(pr => pr.ReturnDate).FirstOrDefault(),
                    VacationDays = _ctx.PermissionRequests.Where(pr => pr.FormRequestId == fr.Id).Select(pr => pr.PermissionDays).FirstOrDefault()
                })
                .ToListAsync(cancellationToken);
            return ServiceResult<List<VacationRequestDTO>>.Success(requests);
        }
    }
}
