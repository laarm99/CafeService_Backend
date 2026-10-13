using Microsoft.EntityFrameworkCore;
using MultipleFormat.Application.Modules.EmployeeInfo.DTOs;
using MultipleFormat.Domain.Entities;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.EmployeeInfo
{
    public class EmployeeInfoService
    {
        private readonly MultipleFormatDbContext _ctx;

        public EmployeeInfoService(MultipleFormatDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ServiceResult<EmployeeInfoDTO>> GetEmployeeInfoByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var employee = await _ctx.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);

            if (employee == null)
                return ServiceResult<EmployeeInfoDTO>.NotFound("Empleado no encontrado");

            var dto = new EmployeeInfoDTO
            {
                CurrentDate = DateTime.UtcNow,
                EmployeeNumber = employee.EmployeeNumber,
                FullName = employee.FullName,
                Position = employee.Position,
                Department = employee.Department,
                SupervisorName = employee.SupervisorName,
                Plant = employee.Plant.HasValue ? employee.Plant.Value.ToString().Replace('_', ' ') : null
            };

            return ServiceResult<EmployeeInfoDTO>.Success(dto);
        }
    }
}
