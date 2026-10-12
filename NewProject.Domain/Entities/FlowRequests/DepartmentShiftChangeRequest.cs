using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultipleFormat.Domain.Entities.FlowRequests
{
    [Table("MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST")]
    public class DepartmentShiftChangeRequest
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("FORM_REQUEST_ID")]
        public int FormRequestId { get; set; }

        [Column("CURRENT_DEPARTMENT")]
        public string CurrentDepartment { get; set; } = string.Empty;

        [Column("NEW_DEPARTMENT")]
        public string NewDepartment { get; set; } = string.Empty;

        [Column("CURRENT_SHIFT")]
        public string CurrentShift { get; set; } = string.Empty;

        [Column("NEW_SHIFT")]
        public string NewShift { get; set; } = string.Empty;

        [Column("CURRENT_PLANT")]
        public string CurrentPlant { get; set; } = string.Empty;

        [Column("NEW_PLANT")]
        public string NewPlant { get; set; } = string.Empty;

        [Column("CURRENT_SUPERVISOR")]
        public string CurrentSupervisor { get; set; } = string.Empty;

        [Column("NEW_SUPERVISOR")]
        public string NewSupervisor { get; set; } = string.Empty;

        [Column("EFFECTIVE_DATE")]
        public DateTime EffectiveDate { get; set; }

        [Column("REASON")]
        public string? Reason { get; set; }
        public virtual FormRequest FormRequest { get; set; } = null!;
    }
}
