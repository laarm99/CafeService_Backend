using MultipleFormat.Application.Modules.VacationsRequest.DTOs;
using MultipleFormat.Domain.Common.Enums;
using MultipleFormat.Domain.Entities.FlowRequests;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MultipleFormat.Application.Modules.VacationsRequest
{
    public class VacationRequestService
    {
        private readonly MultipleFormatDbContext _ctx;

        public VacationRequestService(MultipleFormatDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ServiceResult<int>> CreateVacationRequest(VacationCreateRequestDTO dto, int employeeId)
        {
            // Basic validations
            if (dto.StartDate > dto.EndDate)
            {
                return ServiceResult<int>.BadRequest("StartDate must be before or equal to EndDate.");
            }

            if (dto.VacationDays <= 0)
            {
                return ServiceResult<int>.BadRequest("VacationDays must be greater than zero.");
            }

            if (dto.ReturnDate < dto.EndDate)
            {
                return ServiceResult<int>.BadRequest("ReturnDate cannot be before EndDate.");
            }

            var formRequestData = new FormRequest()
            {
                EmployeeId = employeeId,
                FormType = FormType.Vacation,
                Status = RequestStatus.Submitted,
                CreatedDate = DateTime.UtcNow,
                Comments = dto.Comments
            };

            // Associate VacationRequest using navigation property so EF will set FK on save
            var vacationRequestData = new VacationRequest()
            {
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ReturnDate = dto.ReturnDate,
                VacationDays = dto.VacationDays,
                FormRequest = formRequestData
            };

            _ctx.FormRequests.Add(formRequestData);
            _ctx.VacationRequests.Add(vacationRequestData);

            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Could be constraint violation or concurrency issue
                return ServiceResult<int>.InternalServerError("Error saving vacation request.");
            }

            return ServiceResult<int>.Success(formRequestData.Id);
        }

        public async Task<ServiceResult<List<VacationRequestDTO>>> GetByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var query = from vr in _ctx.VacationRequests.AsNoTracking()
                        join fr in _ctx.FormRequests.AsNoTracking() on vr.FormRequestId equals fr.Id
                        where fr.EmployeeId == employeeId
                        orderby fr.CreatedDate descending
                        select new VacationRequestDTO
                        {
                            FormRequestId = fr.Id,
                            FormType = fr.FormType,
                            Status = fr.Status,
                            CreatedDate = fr.CreatedDate,
                            SubmittedDate = fr.SubmittedDate,
                            Comments = fr.Comments,
                            StartDate = vr.StartDate,
                            EndDate = vr.EndDate,
                            ReturnDate = vr.ReturnDate,
                            VacationDays = vr.VacationDays
                        };

            var list = await query.ToListAsync(cancellationToken);

            return ServiceResult<List<VacationRequestDTO>>.Success(list);
        }
    }
}
