using Microsoft.EntityFrameworkCore;
using MultipleFormat.Domain.Common.Enums;
using NewProject.Application.Common.Email;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;

namespace MultipleFormat.Application.Modules.FormRequests
{
    // Acciones exclusivas del Admin sobre solicitudes ya aprobadas por el supervisor.
    public class FormRequestAdminService
    {
        private readonly MultipleFormatDbContext _ctx;
        private readonly IEmailService _emailService;

        public FormRequestAdminService(MultipleFormatDbContext ctx, IEmailService emailService)
        {
            _ctx = ctx;
            _emailService = emailService;
        }

        public async Task<ServiceResult<bool>> CompleteAsync(
            int id,
            string? domainUserName,
            CancellationToken cancellationToken = default)
        {
            if (!await IsAdminAsync(domainUserName, cancellationToken))
            {
                return ServiceResult<bool>.Forbidden("Solo un administrador puede completar solicitudes.");
            }

            var formRequest = await _ctx.FormRequests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (formRequest == null)
            {
                return ServiceResult<bool>.NotFound("Solicitud no encontrada.");
            }

            if (formRequest.Status != RequestStatus.Approved)
            {
                return ServiceResult<bool>.BadRequest("Solo se pueden completar solicitudes aprobadas por el supervisor.");
            }

            formRequest.Status = RequestStatus.Completed;
            await _ctx.SaveChangesAsync(cancellationToken);

            await NotifyCompletedAsync(formRequest.Id, formRequest.EmployeeId, cancellationToken);
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<byte[]>> GetPdfAsync(
            int id,
            string? domainUserName,
            CancellationToken cancellationToken = default)
        {
            if (!await IsAdminAsync(domainUserName, cancellationToken))
            {
                return ServiceResult<byte[]>.Forbidden("Solo un administrador puede descargar el formato.");
            }

            var formRequest = await _ctx.FormRequests.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (formRequest == null)
            {
                return ServiceResult<byte[]>.NotFound("Solicitud no encontrada.");
            }

            if (formRequest.Status != RequestStatus.Approved && formRequest.Status != RequestStatus.Completed)
            {
                return ServiceResult<byte[]>.BadRequest("El formato solo se puede descargar cuando está aprobado o completado.");
            }

            var employee = await _ctx.Employees.AsNoTracking()
                .FirstAsync(x => x.Id == formRequest.EmployeeId, cancellationToken);

            var data = new MultipleFormatPdfData
            {
                Date = formRequest.CreatedDate,
                EmployeeName = employee.FullName,
                EmployeeNumber = employee.EmployeeNumber,
                HireDate = employee.HireDate,
                Position = employee.Position,
                Department = employee.Department,
                SupervisorName = employee.SupervisorName,
                CurrentPlant = employee.Plant?.ToString(),
                CurrentShift = employee.Shift,
                FormType = formRequest.FormType,
                Comments = formRequest.Comments
            };

            switch (formRequest.FormType)
            {
                case FormType.Vacation:
                    var vacation = await _ctx.VacationRequests.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.FormRequestId == id, cancellationToken);
                    if (vacation != null)
                    {
                        data.Days = vacation.VacationDays;
                        data.StartDate = vacation.StartDate;
                        data.EndDate = vacation.EndDate;
                        data.ReturnDate = vacation.ReturnDate;
                        data.EffectiveDate = vacation.StartDate;
                    }
                    break;

                case FormType.Permission:
                    var permission = await _ctx.PermissionRequests.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.FormRequestId == id, cancellationToken);
                    if (permission != null)
                    {
                        data.PermissionType = permission.PermissionType;
                        data.Days = permission.PermissionDays;
                        data.StartDate = permission.StartDate;
                        data.EndDate = permission.EndDate;
                        data.ReturnDate = permission.ReturnDate;
                        data.EffectiveDate = permission.StartDate;
                        data.Comments = JoinComments(data.Comments, permission.SwapDetail);
                    }
                    break;

                case FormType.TimeAdjustment:
                    var adjustment = await _ctx.TimeAdjustmentRequests.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.FormRequestId == id, cancellationToken);
                    if (adjustment != null)
                    {
                        data.EffectiveDate = adjustment.AdjustmentDate;
                        data.PunchType = adjustment.MissingPunchType.ToString();
                        data.EntryTime = adjustment.EntryTime;
                        data.ExitTime = adjustment.ExitTime;
                        data.Justification = adjustment.Justification;
                    }
                    break;

                case FormType.DepartmentShiftChange:
                    var change = await _ctx.DepartmentShiftChangeRequests.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.FormRequestId == id, cancellationToken);
                    if (change != null)
                    {
                        data.EffectiveDate = change.EffectiveDate;
                        data.NewPlant = change.NewPlant;
                        data.NewShift = change.NewShift;
                        data.NewDepartment = change.NewDepartment;
                        data.NewSupervisor = change.NewSupervisor;
                    }
                    break;
            }

            return ServiceResult<byte[]>.Success(MultipleFormatPdfBuilder.Build(data));
        }

        private static string? JoinComments(string? first, string? second)
        {
            var parts = new[] { first, second }.Where(x => !string.IsNullOrWhiteSpace(x));
            var joined = string.Join(" | ", parts);
            return joined.Length == 0 ? null : joined;
        }

        private async Task<bool> IsAdminAsync(string? domainUserName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(domainUserName))
            {
                return false;
            }

            var userName = domainUserName.Trim();
            var slash = userName.LastIndexOf('\\');
            if (slash >= 0)
            {
                userName = userName[(slash + 1)..];
            }

            userName = userName.ToUpperInvariant();
            return await (from account in _ctx.IDMAccounts.AsNoTracking()
                          join accountRole in _ctx.IDMAccountRoles.AsNoTracking()
                              on account.Id equals accountRole.AccountId
                          join role in _ctx.IDMRoles.AsNoTracking()
                              on accountRole.RoleId equals role.Id
                          where account.IsActive && role.IsActive &&
                                account.DomainUserName == userName &&
                                role.RoleName == nameof(Roles.Admin)
                          select account.Id).AnyAsync(cancellationToken);
        }

        private async Task NotifyCompletedAsync(int formRequestId, int employeeId, CancellationToken cancellationToken)
        {
            try
            {
                var info = await (from employee in _ctx.Employees.AsNoTracking()
                                  where employee.Id == employeeId
                                  select new
                                  {
                                      employee.FullName,
                                      EmployeeEmail = employee.Email,
                                      SupervisorEmail = employee.Supervisor != null ? employee.Supervisor.Email : null
                                  }).FirstOrDefaultAsync(cancellationToken);

                if (info == null) return;

                var recipients = new[] { info.EmployeeEmail, info.SupervisorEmail }
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (recipients.Count == 0) return;

                await _emailService.SendAsync(new EmailMessage
                {
                    To = recipients,
                    Subject = $"Solicitud #{formRequestId} completada",
                    Body = $"La solicitud #{formRequestId} de {info.FullName} fue completada por Recursos Humanos."
                }, cancellationToken);
            }
            catch
            {
                // El correo no debe revertir la finalizacion de la solicitud.
            }
        }
    }
}
