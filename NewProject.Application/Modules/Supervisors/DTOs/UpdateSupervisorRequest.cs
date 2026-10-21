using System.ComponentModel.DataAnnotations;

namespace MultipleFormat.Application.Modules.Supervisors.DTOs
{
    public class UpdateSupervisorRequest
    {
        [Required]
        public string DomainUserName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public bool IsActive { get; set; }
    }
}
