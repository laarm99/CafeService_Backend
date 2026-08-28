using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewProject.Domain.Entities.IDM;
using NewProject.Infrastructure.Persistence;

[Authorize]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ProjectDbContext _ctx;

    public AuthController(ProjectDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        var userName = User.Identity?.Name?
            .Split('\\')
            .Last()
            .ToUpper();

        var account = _ctx.IDMAccounts
            .FirstOrDefault(x =>
                x.DomainUserName == userName &&
                x.IsActive);

        if (account == null)
        {
            return Unauthorized(
                "User is not authorized."
            );
        }

        return Ok(account);
    }
}