using MultipleFormat.Application.Modules.ChangesDepOrShifRequest.DTOs;
using MultipleFormat.Domain.Common.Enums;
using MultipleFormat.Domain.Entities.FlowRequests;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.ChangesDepOrShifRequest
{
    public class ChangesDepOrShiftRequestService
    {
        private readonly MultipleFormatDbContext _ctx;

        public ChangesDepOrShiftRequestService(MultipleFormatDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ServiceResult<int>> CreateChangesDepOrShiftRequests(ChangesDepOrShiftCreateRequestDTO dto)
        {
            FormRequest formRequestData = new()
            {
                EmployeeId = dto.EmployeeId,
                FormType = FormType.DepartmentShiftChange,
                Status = RequestStatus.Submitted,
                CreatedDate = DateTime.UtcNow,                
            };

            _ctx.FormRequests.Add(formRequestData);

            DepartmentShiftChangeRequest changesDepOrShiftRequestData = new()
            {
                FormRequestId = formRequestData.Id,
                CurrentPlant = dto.NewPlant,
                CurrentShift = dto.NewShift,
                CurrentDepartment = dto.NewDepartment,
                CurrentSupervisor = dto.NewSupervisor,
                EffectiveDate = dto.EffectiveDate
            };

            _ctx.DepartmentShiftChangeRequests.Add(changesDepOrShiftRequestData);

            await _ctx.SaveChangesAsync();

            return ServiceResult<int>.Success(formRequestData.Id);
        }
    }
}
