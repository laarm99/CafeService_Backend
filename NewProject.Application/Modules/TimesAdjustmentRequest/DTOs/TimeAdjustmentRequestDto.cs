using MultipleFormat.Domain.Common.Enums;
using System;

namespace MultipleFormat.Application.Modules.TimesAdjustmentRequest.DTOs
{
    public class TimeAdjustmentRequestDto
    {
        public int FormRequestId { get; set; }
        public FormType FormType { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string? Comments { get; set; }

        public DateTime AdjustmentDate { get; set; }
        public MissingPunchType MissingPunchType { get; set; }
        public TimeSpan? EntryTime { get; set; }
        public TimeSpan? ExitTime { get; set; }
    }
}
