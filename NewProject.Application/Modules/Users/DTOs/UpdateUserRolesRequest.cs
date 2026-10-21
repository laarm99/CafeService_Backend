using System.Collections.Generic;

namespace NewProject.Application.Modules.Users.DTOs
{
    public class UpdateUserRolesRequest
    {
        public List<string> Roles { get; set; } = new();
    }
}
