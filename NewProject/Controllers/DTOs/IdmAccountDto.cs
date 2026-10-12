using System;

namespace MultipleFormat.Web.Controllers.DTOs
{
    public class IdmAccountDto
    {
        public string DomainUserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
