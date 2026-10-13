using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.EmployeeInfo.DTOs
{
    public class EmployeeInfoDTO
    {
        public DateTime CurrentDate { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string? SupervisorName { get; set; }
        public string? Plant { get; set; }
    }
}
