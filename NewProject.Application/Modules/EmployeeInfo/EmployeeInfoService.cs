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
            EmployeeInfoDTO? dto = await _ctx.Employees
                 .Where(e => e.Id == employeeId)
                 .Select(e => new EmployeeInfoDTO
                 {
                     CurrentDate = DateTime.UtcNow,
                     EmployeeNumber = e.EmployeeNumber,
                     FullName = e.FullName,
                     Position = e.Position,
                     Department = e.Department
                 })
                 .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
                return ServiceResult<EmployeeInfoDTO>.NotFound("Empleado no encontrado");

            return ServiceResult<EmployeeInfoDTO>.Success(dto);
        }
    }
}
