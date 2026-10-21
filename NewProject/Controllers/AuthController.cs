using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleFormat.Domain.Common.Enums;
using MultipleFormat.Domain.Entities;
using NewProject.Domain.Entities.IDM;
using NewProject.Infrastructure.Persistence;
using MultipleFormat.Web.Controllers.DTOs;
using NewProject.Application.Common.CurrentUser;
using NewProject.Common.Results;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private static readonly string DefaultRoleName =
        nameof(Roles.GeneralUser);
    private readonly MultipleFormatDbContext _ctx;
    private readonly ICurrentUserService _currentUserService;

    public AuthController(MultipleFormatDbContext ctx, ICurrentUserService currentUserService)
    {
        _ctx = ctx;
        _currentUserService = currentUserService;
    }

    [AllowAnonymous]
    [HttpGet("login")]
    public async Task<IActionResult> Login(CancellationToken cancellationToken)
    {
        ServiceResult<CurrentUserDto> currentUserResult = await _currentUserService.GetCurrentEmployeeAsync(User, cancellationToken);

        if (!currentUserResult.IsSuccess || currentUserResult.Data == null)
        {
            IIdentity identity = User.Identity;
            if (identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }

            return StatusCode(StatusCodes.Status403Forbidden, new { message = "El usuario no tiene acceso habilitado a la aplicación." });
        }

        CurrentUserDto employee = currentUserResult.Data;
        string userName = employee.DomainUserName;

        // Buscar también cuentas inactivas para no duplicarlas.
        IDMAccount account = await _ctx.IDMAccounts
            .FirstOrDefaultAsync(
                x => x.DomainUserName == userName,
                cancellationToken);

        if (account != null)
        {
            if (!account.IsActive)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new
                    {
                        message =
                            "El usuario no tiene acceso habilitado a la aplicación."
                    });
            }

            var roles = await GetActiveRoleNamesAsync(account.Id, cancellationToken);

            // Mantener la cuenta y sus roles existentes. Devolver DTO para no exponer la entidad EF directamente.
            IdmAccountDto existingDto = new IdmAccountDto
            {
                DomainUserName = account.DomainUserName,
                FullName = account.FullName,
                Email = account.Email,
                IsActive = account.IsActive,
                Roles = roles,
                CreatedDate = account.CreatedDate
            };

            return Ok(existingDto);
        }

        // Buscar GeneralUser en IDM_ROLE.
        IDMRole defaultRole = await _ctx.IDMRoles
            .FirstOrDefaultAsync(
                x => x.RoleName == DefaultRoleName &&
                     x.IsActive,
                cancellationToken);

        if (defaultRole == null)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    message =
                        "El acceso no está disponible por un problema de configuración."
                });
        }

        // Crear la cuenta con los datos de MFR_EMPLOYEES.
        IDMAccount newAccount = new IDMAccount
        {
            DomainUserName = userName,
            FullName = employee.FullName,
            Email = employee.Email,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        // Relacionar la cuenta nueva con GeneralUser.
        // EF Core utilizará el Id generado para la cuenta
        // y el Id del rol encontrado en la tabla.
        IDMAccountRole accountRole = new IDMAccountRole
        {
            Account = newAccount,
            Role = defaultRole
        };

        _ctx.IDMAccounts.Add(newAccount);
        _ctx.IDMAccountRoles.Add(accountRole);

        // Guardar cuenta y asignación de rol juntas.
        try
        {
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            // Posible creación concurrente: intentar recuperar la cuenta existente y devolverla.
            IDMAccount existing = await _ctx.IDMAccounts
                .FirstOrDefaultAsync(x => x.DomainUserName == userName, cancellationToken);

            if (existing != null)
            {
                var roles = await GetActiveRoleNamesAsync(existing.Id, cancellationToken);
                IdmAccountDto existingDto = new IdmAccountDto
                {
                    DomainUserName = existing.DomainUserName,
                    FullName = existing.FullName,
                    Email = existing.Email,
                    IsActive = existing.IsActive,
                    Roles = roles,
                    CreatedDate = existing.CreatedDate
                };

                return Ok(existingDto);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error creando la cuenta." });
        }

        IdmAccountDto newDto = new IdmAccountDto
        {
            DomainUserName = newAccount.DomainUserName,
            FullName = newAccount.FullName,
            Email = newAccount.Email,
            IsActive = newAccount.IsActive,
            Roles = new List<string> { defaultRole.RoleName },
            CreatedDate = newAccount.CreatedDate
        };

        return Ok(newDto);

    }

    private async Task<List<string>> GetActiveRoleNamesAsync(
        int accountId,
        CancellationToken cancellationToken)
    {
        return await (from accountRole in _ctx.IDMAccountRoles.AsNoTracking()
                      join role in _ctx.IDMRoles.AsNoTracking()
                          on accountRole.RoleId equals role.Id
                      where accountRole.AccountId == accountId && role.IsActive
                      select role.RoleName)
            .ToListAsync(cancellationToken);
    }
}