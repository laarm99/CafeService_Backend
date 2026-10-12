using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;

namespace NewProject.Application.Common.CurrentUser
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly MultipleFormatDbContext _ctx;

        public CurrentUserService(MultipleFormatDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ServiceResult<CurrentUserDto>> GetCurrentEmployeeAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var identity = user?.Identity;
            if (identity?.IsAuthenticated != true)
            {
                return ServiceResult<CurrentUserDto>.NotFound("User not authenticated.");
            }

            var windowsName = identity.Name?.Trim();
            if (string.IsNullOrWhiteSpace(windowsName))
            {
                return ServiceResult<CurrentUserDto>.NotFound("Invalid user name.");
            }

            var parts = windowsName.Split('\\');
            string userName;
            if (parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[1]))
            {
                userName = parts[1].Trim();
            }
            else
            {
                userName = windowsName;
            }

            userName = userName.ToUpperInvariant();

            var employee = await _ctx.Set<MultipleFormat.Domain.Entities.Employee>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.DomainUserName == userName && x.IsActive, cancellationToken);

            if (employee == null)
            {
                return ServiceResult<CurrentUserDto>.NotFound("Employee not found or inactive.");
            }

            var dto = new CurrentUserDto
            {
                Id = employee.Id,
                EmployeeNumber = employee.EmployeeNumber,
                DomainUserName = employee.DomainUserName,
                FullName = employee.FullName,
                Email = employee.Email,
                IsActive = employee.IsActive
            };

            return ServiceResult<CurrentUserDto>.Success(dto);
        }
    }
}
