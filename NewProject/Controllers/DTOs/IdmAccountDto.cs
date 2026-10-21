using System;
using System.Collections.Generic;

namespace MultipleFormat.Web.Controllers.DTOs
{
    public class IdmAccountDto
    {
        public string DomainUserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = new();
        public DateTime CreatedDate { get; set; }
    }
}
