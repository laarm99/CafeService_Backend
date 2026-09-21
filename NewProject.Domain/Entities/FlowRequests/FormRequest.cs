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
        public int Id { get; set; }
        public string EmployeeId { get; set; } = string.Empty;
        public FormType FormType { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string? Comments { get; set; }
    }
}
