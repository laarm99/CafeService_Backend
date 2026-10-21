using System.ComponentModel.DataAnnotations;

namespace NewProject.Application.Modules.Users.DTOs
{
    public class UpdateUserRequest
    {
        [Required]
        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
