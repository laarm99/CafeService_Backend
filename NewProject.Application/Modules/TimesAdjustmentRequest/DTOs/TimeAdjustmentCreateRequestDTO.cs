using MultipleFormat.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.TimesAdjustmentRequest.DTOs
{
    public class TimeAdjustmentCreateRequestDTO
    {
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime AdjustmentDate { get; set; }
        public MissingPunchType MissingPunchType { get; set; } 
        public TimeSpan? EntryTime { get; set; }
        public TimeSpan? ExitTime { get; set; }        
    }
}
