using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using NewProject.Common.Results;

namespace NewProject.Application.Common.CurrentUser
{
    public interface ICurrentUserService
    {
        Task<ServiceResult<CurrentUserDto>> GetCurrentEmployeeAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
    }
}
