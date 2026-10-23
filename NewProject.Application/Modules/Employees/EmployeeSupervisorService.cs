using MultipleFormat.Domain.Entities;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultipleFormat.Application.Modules.Employees
{
    /// <summary>
    /// Servicio que gestiona la validación y creación de supervisores
    /// cuando se agregan nuevos empleados.
    /// </summary>
    public class EmployeeSupervisorService
    {
        private readonly MultipleFormatDbContext _ctx;

        public EmployeeSupervisorService(MultipleFormatDbContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Valida y crea el supervisor si no existe cuando se agrega un empleado.
        /// </summary>
        public async Task<ServiceResult<int?>> ValidateAndCreateSupervisorIfNeededAsync(
            int employeeId,
            string? supervisorName,
            CancellationToken cancellationToken = default)
        {
            // Si no hay supervisor asignado, retorna null
            if (string.IsNullOrWhiteSpace(supervisorName))
            {
                return ServiceResult<int?>.Success(null);
            }

            try
            {
                // Normalizar el nombre para búsqueda (eliminar espacios extra, convertir a mayúsculas)
                string normalizedName = NormalizeName(supervisorName);

                // Buscar si el supervisor ya existe
                var existingSupervisor = await _ctx.Supervisors
                    .FirstOrDefaultAsync(
                        s => s.NormalizedName == normalizedName,
                        cancellationToken);

                if (existingSupervisor != null)
                {
                    // El supervisor existe, solo actualizar el empleado si es necesario
                    return ServiceResult<int?>.Success(existingSupervisor.Id);
                }

                // Crear nuevo supervisor
                var newSupervisor = new Supervisor
                {
                    FullName = supervisorName,
                    NormalizedName = normalizedName,
                    DomainUserName = null, // Se asignará cuando el supervisor entre a la app
                    Email = null,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                _ctx.Supervisors.Add(newSupervisor);
                await _ctx.SaveChangesAsync(cancellationToken);

                return ServiceResult<int?>.Success(newSupervisor.Id);
            }
            catch (Exception ex)
            {
                return ServiceResult<int?>.InternalServerError(
                    $"Error al procesar el supervisor: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca un supervisor por nombre normalizado.
        /// </summary>
        public async Task<Supervisor?> GetSupervisorByNormalizedNameAsync(
            string supervisorName,
            CancellationToken cancellationToken = default)
        {
            string normalizedName = NormalizeName(supervisorName);
            return await _ctx.Supervisors
                .FirstOrDefaultAsync(
                    s => s.NormalizedName == normalizedName && s.IsActive,
                    cancellationToken);
        }

        /// <summary>
        /// Normaliza el nombre del supervisor para búsqueda consistente.
        /// </summary>
        private string NormalizeName(string name)
        {
            return name
                .Trim()
                .ToUpperInvariant()
                .Replace("  ", " "); // Eliminar espacios dobles
        }
    }
}
