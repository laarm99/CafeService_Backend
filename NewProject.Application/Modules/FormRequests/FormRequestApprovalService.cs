using Microsoft.EntityFrameworkCore;
using MultipleFormat.Application.Modules.FormRequests.DTOs;
using MultipleFormat.Domain.Common.Enums;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;
using MultipleFormat.Application.Modules.Supervisors;

namespace MultipleFormat.Application.Modules.FormRequests
{
    public class FormRequestApprovalService
    {
        private readonly MultipleFormatDbContext _ctx;

        public FormRequestApprovalService(MultipleFormatDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<int?> ResolveApproverIdAsync(
            int employeeId,
            CancellationToken cancellationToken = default)
        {
            var supervisorUserName = await _ctx.Employees
                .Where(x => x.Id == employeeId)
                .Select(x => x.Supervisor != null ? x.Supervisor.DomainUserName : null)
                .FirstOrDefaultAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(supervisorUserName))
            {
                return null;
            }

            var normalizedUserName = supervisorUserName.Trim().ToUpperInvariant();

            return await _ctx.IDMAccounts
                .Where(x => x.IsActive && x.DomainUserName == normalizedUserName)
                .Select(x => (int?)x.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<ServiceResult<bool>> ApproveAsync(
            int formRequestId,
            string? domainUserName,
            FormRequestDecisionDto decision,
            CancellationToken cancellationToken = default)
        {
            return DecideAsync(formRequestId, domainUserName, RequestStatus.Approved, decision, cancellationToken);
        }

        public Task<ServiceResult<bool>> RejectAsync(
            int formRequestId,
            string? domainUserName,
            FormRequestDecisionDto decision,
            CancellationToken cancellationToken = default)
        {
            return DecideAsync(formRequestId, domainUserName, RequestStatus.Rejected, decision, cancellationToken);
        }

        private async Task<ServiceResult<bool>> DecideAsync(
            int formRequestId,
            string? domainUserName,
            RequestStatus targetStatus,
            FormRequestDecisionDto decision,
            CancellationToken cancellationToken)
        {
            var normalizedUserName = SupervisorService.NormalizeDomainUserName(domainUserName);
            var supervisor = await (from account in _ctx.IDMAccounts.AsNoTracking()
                                    join accountRole in _ctx.IDMAccountRoles.AsNoTracking()
                                        on account.Id equals accountRole.AccountId
                                    join role in _ctx.IDMRoles.AsNoTracking()
                                        on accountRole.RoleId equals role.Id
                                    join candidate in _ctx.Supervisors.AsNoTracking()
                                        on account.DomainUserName equals candidate.DomainUserName
                                    where account.IsActive && role.IsActive &&
                                          role.RoleName == nameof(Roles.Supervisor) &&
                                          account.DomainUserName == normalizedUserName &&
                                          candidate.IsActive
                                    select new { AccountId = account.Id, SupervisorId = candidate.Id, DomainUserName = candidate.DomainUserName })
                .FirstOrDefaultAsync(cancellationToken);

            if (supervisor == null)
            {
                return ServiceResult<bool>.Forbidden("El usuario no tiene permiso para decidir solicitudes.");
            }

            var isAdmin = await (from accountRole in _ctx.IDMAccountRoles.AsNoTracking()
                                 join role in _ctx.IDMRoles.AsNoTracking()
                                     on accountRole.RoleId equals role.Id
                                 where accountRole.AccountId == supervisor.AccountId &&
                                       role.IsActive &&
                                       role.RoleName == nameof(Roles.Admin)
                                 select accountRole.AccountId).AnyAsync(cancellationToken);

            if (isAdmin)
            {
                // Un usuario Admin + Supervisor puede aprobar si la solicitud NO es propia
                // Pero si es propia, rechaza porque requiere aprobación de otro Supervisor
                var ownRequest = await (from formRequest in _ctx.FormRequests
                                        join employee in _ctx.Employees
                                            on formRequest.EmployeeId equals employee.Id
                                        join idmAccount in _ctx.IDMAccounts
                                            on supervisor.AccountId equals idmAccount.Id
                                        where formRequest.Id == formRequestId
                                        select new { FormRequest = formRequest, Employee = employee, IDMUserName = idmAccount.DomainUserName })
                    .FirstOrDefaultAsync(cancellationToken);

                if (ownRequest != null && string.Equals(ownRequest.Employee.DomainUserName, ownRequest.IDMUserName, StringComparison.OrdinalIgnoreCase))
                {
                    return ServiceResult<bool>.BadRequest("Tu solicitud requiere aprobación de otro Supervisor.");
                }
                // Si no es propia, permite que Admin+Supervisor continúe como Supervisor
            }

            var request = await (from formRequest in _ctx.FormRequests
                                 join employee in _ctx.Employees
                                     on formRequest.EmployeeId equals employee.Id
                                 where formRequest.Id == formRequestId
                                 select new { FormRequest = formRequest, Employee = employee })
                .FirstOrDefaultAsync(cancellationToken);

            if (request == null)
            {
                return ServiceResult<bool>.NotFound("Solicitud no encontrada.");
            }

            if (request.FormRequest.FormType == FormType.DepartmentShiftChange)
            {
                return ServiceResult<bool>.BadRequest("Este tipo de solicitud no está habilitado para aprobación.");
            }

            if (request.FormRequest.Status != RequestStatus.Submitted)
            {
                return ServiceResult<bool>.BadRequest("La solicitud no se encuentra pendiente de decisión.");
            }

            if (request.Employee.SupervisorId != supervisor.SupervisorId)
            {
                return ServiceResult<bool>.Forbidden("La solicitud no corresponde a un empleado a cargo.");
            }

            if (string.Equals(request.Employee.DomainUserName, supervisor.DomainUserName, StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult<bool>.BadRequest("No se puede aprobar o rechazar una solicitud propia.");
            }

            request.FormRequest.Status = targetStatus;
            request.FormRequest.ApproverId = supervisor.AccountId;
            if (!string.IsNullOrWhiteSpace(decision.Comments))
            {
                request.FormRequest.Comments = decision.Comments.Trim();
            }

            await _ctx.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.Success(true);
        }
    }
}
