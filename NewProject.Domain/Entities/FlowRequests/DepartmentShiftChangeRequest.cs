using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultipleFormat.Domain.Entities.FlowRequests
{
    [Table("MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST")]
    public class DepartmentShiftChangeRequest
    {
        public int Id { get; set; }

        public int FormRequestId { get; set; }

        public string CurrentDepartment { get; set; } = string.Empty;

        public string NewDepartment { get; set; } = string.Empty;

        public string CurrentShift { get; set; } = string.Empty;

        public string NewShift { get; set; } = string.Empty;

        public DateTime EffectiveDate { get; set; }

        public string? Reason { get; set; }

        public virtual FormRequest FormRequest { get; set; } = null!;
    }
}
