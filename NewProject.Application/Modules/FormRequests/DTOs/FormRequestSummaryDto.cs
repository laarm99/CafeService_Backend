using MultipleFormat.Domain.Common.Enums;

namespace MultipleFormat.Application.Modules.FormRequests.DTOs
{
    public class FormRequestSummaryDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public string EmployeeFullName { get; set; } = string.Empty;
        public FormType FormType { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string? Comments { get; set; }
    }
}
