using MultipleFormat.Application.Modules.ChangesDepOrShifRequest.DTOs;
using MultipleFormat.Domain.Common.Enums;
using MultipleFormat.Domain.Entities.FlowRequests;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                FormRequest = formRequestData,
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

        public async Task<ServiceResult<List<ChangesDepOrShiftRequestDTO>>> GetChangesDepOrShiftRequestByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var requests = await (from fr in _ctx.FormRequests
                                  join ds in _ctx.DepartmentShiftChangeRequests on fr.Id equals ds.FormRequestId
                                  where fr.EmployeeId == employeeId && fr.FormType == FormType.DepartmentShiftChange
                                  select new ChangesDepOrShiftRequestDTO
                                  {
                                      FormRequestId = fr.Id,
                                      FormType = fr.FormType,
                                      Status = fr.Status,
                                      CreatedDate = fr.CreatedDate,
                                      SubmittedDate = fr.SubmittedDate,
                                      CurrentPlant = ds.CurrentPlant,
                                      CurrentShift = ds.CurrentShift,
                                      CurrentDepartment = ds.CurrentDepartment,
                                      CurrentSupervisor = ds.CurrentSupervisor,
                                      EffectiveDate = ds.EffectiveDate
                                  }).ToListAsync(cancellationToken);

            return ServiceResult<List<ChangesDepOrShiftRequestDTO>>.Success(requests);
        }
    }
}
