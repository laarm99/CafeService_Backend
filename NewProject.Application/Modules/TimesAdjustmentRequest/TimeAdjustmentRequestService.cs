using MultipleFormat.Application.Modules.TimesAdjustmentRequest.DTOs;
using MultipleFormat.Domain.Common.Enums;
using MultipleFormat.Domain.Entities.FlowRequests;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace MultipleFormat.Application.Modules.TimesAdjustmentRequest
{    
    public class TimeAdjustmentRequestService
    {
        private readonly MultipleFormatDbContext _ctx;

        public TimeAdjustmentRequestService(MultipleFormatDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ServiceResult<int>> CreateTimeAdjustmentRequest(TimeAdjustmentCreateRequestDTO dto, int employeeId)
        {
            var formRequestData = new FormRequest()
            {
                EmployeeId = employeeId,
                FormType = FormType.TimeAdjustment,
                Status = RequestStatus.Submitted,
                CreatedDate = DateTime.UtcNow,
                SubmittedDate = DateTime.UtcNow,
                SubmittedById = employeeId
            };

            var timeAdjustmentRequestData = new TimeAdjustmentRequest()
            {
                AdjustmentDate = dto.AdjustmentDate,
                MissingPunchType = dto.MissingPunchType,
                EntryTime = dto.EntryTime,
                ExitTime = dto.ExitTime,
                FormRequest = formRequestData
            };

            _ctx.FormRequests.Add(formRequestData);
            _ctx.TimeAdjustmentRequests.Add(timeAdjustmentRequestData);

            try
            {
                await _ctx.SaveChangesAsync();
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
                            Status = (int)fr.Status,
                            CreatedDate = fr.CreatedDate,
                            SubmittedDate = fr.SubmittedDate,
                            Comments = fr.Comments,
                            AdjustmentDate = ta.AdjustmentDate,
                            MissingPunchType = ta.MissingPunchType,
                            EntryTime = ta.EntryTime,
                            ExitTime = ta.ExitTime
                        };

            List<TimeAdjustmentRequestDto> list = await query.ToListAsync(cancellationToken);

            return ServiceResult<List<TimeAdjustmentRequestDto>>.Success(list);
        }

    }
}
