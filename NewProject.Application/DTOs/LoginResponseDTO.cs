using System;
using System.Collections.Generic;
using System.Text;

namespace NewProject.Application.DTOs
{
    public class LoginResponseDTO
    {
        public int Id { get; set; }

        public string DomainUserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}
