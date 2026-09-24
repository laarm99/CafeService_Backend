using MultipleFormat.Application.Modules.TimesAdjustmentRequest.DTOs;
using MultipleFormat.Domain.Common.Enums;
using MultipleFormat.Domain.Entities.FlowRequests;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.TimesAdjustmentRequest
{    
    public class TimeAdjustmentRequestService
    {
        private readonly MultipleFormatDbContext _ctx;

        public TimeAdjustmentRequestService(MultipleFormatDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ServiceResult<int>> CreateTimeAdjustmentRequest(TimeAdjustmentCreateRequestDTO dto)
        {
            FormRequest formRequestData = new()
            {
                EmployeeId = dto.EmployeeId,
                FormType = FormType.DepartmentShiftChange,
                Status = RequestStatus.Submitted,
                CreatedDate = DateTime.UtcNow,
            };

            _ctx.FormRequests.Add(formRequestData);

            TimeAdjustmentRequest timeAdjustmentRequestData = new()
            {
                FormRequestId = formRequestData.Id,
                AdjustmentDate = dto.AdjustmentDate,
                MissingPunchType = dto.MissingPunchType,
                EntryTime = dto.EntryTime,
                ExitTime = dto.ExitTime
            };

            _ctx.TimeAdjustmentRequests.Add(timeAdjustmentRequestData);
            await _ctx.SaveChangesAsync();
            return ServiceResult<int>.Success(formRequestData.Id);
        }

    }
}
