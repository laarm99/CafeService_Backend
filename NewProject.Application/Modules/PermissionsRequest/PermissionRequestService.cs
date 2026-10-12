
using Microsoft.EntityFrameworkCore;
using MultipleFormat.Application.Modules.PermissionsRequest.DTOs;
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

        public PermissionRequestService(MultipleFormatDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ServiceResult<int>> CreatePermissionRequest(PermissionCreateRequestDTO dto)
        {
            FormRequest formRequestData = new()
            {
                EmployeeId = dto.EmployeeId,
                FormType = FormType.Permission,
                Status = RequestStatus.Submitted,
                CreatedDate = DateTime.UtcNow,
                Comments = dto.Comments
            };

            _ctx.FormRequests.Add(formRequestData);

            await _ctx.SaveChangesAsync();


            PermissionRequest permissionRequestData = new()
            {
                FormRequestId = formRequestData.Id,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                PermissionDays = dto.PermissionDays
            };

            _ctx.PermissionRequests.Add(permissionRequestData);

            await _ctx.SaveChangesAsync();

            return ServiceResult<int>.Success(formRequestData.Id);
        }

        public async Task<ServiceResult<List<VacationRequestDTO>>> GetPermissionRequestByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var requests = await _ctx.FormRequests
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
