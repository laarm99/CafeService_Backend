using MultipleFormat.Application.Modules.ChangesDepOrShifRequest.DTOs;
using MultipleFormat.Application.Modules.FormRequests;
using MultipleFormat.Domain.Common.Enums;
using MultipleFormat.Domain.Entities.FlowRequests;
using MultipleFormat.Domain.Entities;
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
        private readonly FormRequestApprovalService _approvalService;

        public ChangesDepOrShiftRequestService(MultipleFormatDbContext ctx, FormRequestApprovalService approvalService)
        {
            _ctx = ctx;
            _approvalService = approvalService;
        }

        public async Task<ServiceResult<int>> CreateChangesDepOrShiftRequests(ChangesDepOrShiftCreateRequestDTO dto, int createdById, CancellationToken cancellationToken = default)
        {
            FormRequest formRequestData = new()
            {
                EmployeeId = dto.EmployeeId,
                CreatedById = createdById,
                ApproverId = await _approvalService.ResolveApproverIdAsync(dto.EmployeeId, cancellationToken),
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

            await _ctx.SaveChangesAsync(cancellationToken);

            return ServiceResult<int>.Success(formRequestData.Id);
        }

        public async Task<ServiceResult<List<SupervisedEmployeeDto>>> GetSupervisedEmployeesAsync(
            int supervisorId,
            CancellationToken cancellationToken = default)
        {
            var employees = await _ctx.Employees
                .AsNoTracking()
                .Where(x => x.SupervisorId == supervisorId && x.IsActive)
                .OrderBy(x => x.FullName)
                .Select(x => new SupervisedEmployeeDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    EmployeeNumber = x.EmployeeNumber,
                    DomainUser = x.DomainUserName,
                    Plant = x.Plant.HasValue ? x.Plant.Value.ToString() : null,
                    Shift = x.Shift,
                    Department = x.Department,
                    SupervisorName = x.SupervisorName
                })
                .ToListAsync(cancellationToken);

            return ServiceResult<List<SupervisedEmployeeDto>>.Success(employees);
        }

        public Task<bool> IsEmployeeUnderSupervisorAsync(
            int supervisorId,
            int employeeId,
            CancellationToken cancellationToken = default)
        {
            return _ctx.Employees
                .AsNoTracking()
                .AnyAsync(x => x.Id == employeeId &&
                              x.SupervisorId == supervisorId &&
                              x.IsActive,
                    cancellationToken);
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
