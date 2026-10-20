using System;

namespace NewProject.Application.Common.CurrentUser
{
    public class CurrentUserDto
    {
        public int Id { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public string DomainUserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? AccountId { get; set; }
        public int? SupervisorId { get; set; }
        public bool IsSupervisor { get; set; }
    }
}
