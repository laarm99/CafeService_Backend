using MultipleFormat.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.TimesAdjustmentRequest.DTOs
{
    public class TimeAdjustmentCreateRequestDTO
    {
        public DateTime AdjustmentDate { get; set; }
        public MissingPunchType MissingPunchType { get; set; } 
        public TimeSpan? EntryTime { get; set; }
        public TimeSpan? ExitTime { get; set; }        
        public string Justification { get; set; } = string.Empty;
    }
}
