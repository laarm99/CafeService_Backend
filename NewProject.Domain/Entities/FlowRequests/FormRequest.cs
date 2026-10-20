using MultipleFormat.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultipleFormat.Domain.Entities.FlowRequests
{
    [Table("MFR_FORM_REQUEST")]
    public class FormRequest
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("EMPLOYEE_ID")]
        public int EmployeeId { get; set; }

        [Column("CREATED_BY_ID")]
        public int? CreatedById { get; set; }

        [Column("APPROVER_ID")]
        public int? ApproverId { get; set; }

        [Column("FORM_TYPE")]
        public FormType FormType { get; set; }

        [Column("STATUS")]
        public RequestStatus Status { get; set; }

        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; }

        [Column("SUBMITTED_DATE")]
        public DateTime? SubmittedDate { get; set; }

        [Column("SUBMITTED_BY_ID")]
        public int SubmittedById { get; set; }

        [Column("COMMENTS")]
        public string? Comments { get; set; }

        public virtual Employee Employee { get; set; } = null!;

    }
}
