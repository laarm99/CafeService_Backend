using MultipleFormat.Domain.Common.Enums;
using System;

namespace MultipleFormat.Application.Modules.ChangesDepOrShifRequest.DTOs
{
    public class ChangesDepOrShiftRequestDTO
    {
        public int FormRequestId { get; set; }
        public FormType FormType { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }

        public string CurrentPlant { get; set; } = string.Empty;
        public string CurrentShift { get; set; } = string.Empty;
        public string CurrentDepartment { get; set; } = string.Empty;
        public string CurrentSupervisor { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
    }
}
