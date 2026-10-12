using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.PermissionsRequest.DTOs
{
    public class PermissionCreateRequestDTO
    {
        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int PermissionDays { get; set; }

        public string? Comments { get; set; }
    }
}
