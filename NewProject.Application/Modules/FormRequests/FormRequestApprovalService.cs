using Microsoft.EntityFrameworkCore;
using NewProject.Infrastructure.Persistence;

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
    }
}
