using MultipleFormat.Application.Modules.VacationsRequest.DTOs;
using MultipleFormat.Domain.Common.Enums;
using MultipleFormat.Domain.Entities.FlowRequests;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.VacationsRequest
{
    public class VacationRequestService
    {
        private readonly MultipleFormatDbContext _ctx;

        public VacationRequestService(MultipleFormatDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ServiceResult<int>> CreateVacationRequest(VacationCreateRequestDTO dto)
        {
            FormRequest formRequestData = new()
            {
                EmployeeId = dto.EmployeeId,
                FormType = FormType.Vacation,
                Status = RequestStatus.Submitted,
                CreatedDate = DateTime.UtcNow,
                Comments = dto.Comments
            };

            _ctx.FormRequests.Add(formRequestData);            

            VacationRequest vacationRequestData = new()
            {
                FormRequestId = formRequestData.Id,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ReturnDate = dto.ReturnDate,
                VacationDays = dto.VacationDays
            };  
            
            _ctx.VacationRequests.Add(vacationRequestData);
            await _ctx.SaveChangesAsync();

            return ServiceResult<int>.Success(formRequestData.Id);
        }
    }
}
