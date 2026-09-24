using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.VacationsRequest.DTOs
{
    public class VacationCreateRequestDTO
    {
        public string EmployeeId { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public int VacationDays { get; set; }

        public string? Comments { get; set; }
    }
}
