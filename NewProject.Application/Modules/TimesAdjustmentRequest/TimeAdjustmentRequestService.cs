using MultipleFormat.Application.Modules.TimesAdjustmentRequest.DTOs;
using MultipleFormat.Application.Modules.FormRequests;
using MultipleFormat.Domain.Common.Enums;
using MultipleFormat.Domain.Entities.FlowRequests;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MultipleFormat.Application.Modules.TimesAdjustmentRequest
{    
    public class TimeAdjustmentRequestService
    {
        private readonly MultipleFormatDbContext _ctx;
        private readonly FormRequestApprovalService _approvalService;

        public TimeAdjustmentRequestService(MultipleFormatDbContext ctx, FormRequestApprovalService approvalService)
        {
            _ctx = ctx;
            _approvalService = approvalService;
        }

        public async Task<ServiceResult<int>> CreateTimeAdjustmentRequest(TimeAdjustmentCreateRequestDTO dto, int employeeId, int createdById, CancellationToken cancellationToken = default)
        {
            var dateError = NewProject.Application.Common.Rules.RequestDateRules.ValidateAdjustmentDate(dto.AdjustmentDate);
            if (dateError != null)
            {
                return ServiceResult<int>.BadRequest(dateError);
            }

            FormRequest formRequestData = new()
            {
                EmployeeId = employeeId,
                CreatedById = createdById,
                ApproverId = await _approvalService.ResolveApproverIdAsync(employeeId, cancellationToken),
                FormType = FormType.TimeAdjustment,
                Status = RequestStatus.Submitted,
                CreatedDate = DateTime.UtcNow,
                SubmittedDate = DateTime.UtcNow,
                SubmittedById = employeeId
            };

            TimeAdjustmentRequest timeAdjustmentRequestData = new()
            {
                AdjustmentDate = dto.AdjustmentDate,
                MissingPunchType = dto.MissingPunchType,
                EntryTime = dto.EntryTime,
                ExitTime = dto.ExitTime,
                FormRequest = formRequestData,
                Justification = dto.Justification
            };

            _ctx.FormRequests.Add(formRequestData);
            _ctx.TimeAdjustmentRequests.Add(timeAdjustmentRequestData);

            try
            {
                await _ctx.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return ServiceResult<int>.InternalServerError("Error saving time adjustment request.");
            }

            return ServiceResult<int>.Success(formRequestData.Id);
        }

        public async Task<ServiceResult<List<TimeAdjustmentRequestDto>>> GetByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var query = from ta in _ctx.TimeAdjustmentRequests.AsNoTracking()
                        join fr in _ctx.FormRequests.AsNoTracking() on ta.FormRequestId equals fr.Id
                        where fr.EmployeeId == employeeId
                        orderby fr.CreatedDate descending
                        select new TimeAdjustmentRequestDto
                        {
                            FormRequestId = fr.Id,
                            FormType = fr.FormType,
                            Status = fr.Status,
                            CreatedDate = fr.CreatedDate,
                            SubmittedDate = fr.SubmittedDate,
                            Comments = ta.Justification,
                            AdjustmentDate = ta.AdjustmentDate,
                            MissingPunchType = ta.MissingPunchType,
                            EntryTime = ta.EntryTime,
                            ExitTime = ta.ExitTime,                            
                        };

            List<TimeAdjustmentRequestDto> list = await query.ToListAsync(cancellationToken);

            return ServiceResult<List<TimeAdjustmentRequestDto>>.Success(list);
        }

    }
}
