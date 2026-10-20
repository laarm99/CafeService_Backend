using MultipleFormat.Domain.Common.Enums;

namespace MultipleFormat.Application.Modules.FormRequests.DTOs
{
    public class FormRequestDetailDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public string EmployeeFullName { get; set; } = string.Empty;
        public string DomainUserName { get; set; } = string.Empty;
        public FormType FormType { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public ApproverDto? Approver { get; set; }
        public string? Comments { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public string? NewPlant { get; set; }
        public string? NewShift { get; set; }
        public string? NewDepartment { get; set; }
        public string? NewSupervisor { get; set; }
        public string? NewArea { get; set; }

        public int? VacationDays { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public PermissionType? PermissionType { get; set; }
        public int? PermissionDays { get; set; }
        public string? SwapDetail { get; set; }

        public DateTime? AdjustmentDate { get; set; }
        public MissingPunchType? MissingPunchType { get; set; }
        public TimeSpan? EntryTime { get; set; }
        public TimeSpan? ExitTime { get; set; }
        public string? Justification { get; set; }

        public List<FormRequestAttachmentDto> Attachments { get; set; } = new();
    }
}
