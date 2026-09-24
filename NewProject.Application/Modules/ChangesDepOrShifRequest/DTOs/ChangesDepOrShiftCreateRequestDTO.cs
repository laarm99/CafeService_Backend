using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.ChangesDepOrShifRequest.DTOs
{
    public class ChangesDepOrShiftCreateRequestDTO
    {
        public string EmployeeId { get; set; } = string.Empty;      
        public string NewPlant { get; set; } = string.Empty;
        public string NewShift { get; set; } = string.Empty;
        public string NewDepartment { get; set; } = string.Empty;
        public string NewSupervisor { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
    }
}
