
using MultipleFormat.Application.Modules.PermissionsRequest.DTOs;
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
    }
}
