using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.PermissionsRequest.DTOs
{
    public class PermissionCreateRequestDTO
    {
        public string EmployeeId { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int PermissionDays { get; set; }

        public string? Comments { get; set; }
    }
}
