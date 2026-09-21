using MultipleFormat.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultipleFormat.Domain.Entities.FlowRequests
{
    [Table("MFR_TIME_ADJUSTMENT_REQUEST")]
    public class TimeAdjustmentRequest
    {
        public int Id { get; set; }

        public int FormRequestId { get; set; }

        public DateTime AdjustmentDate { get; set; }

        public MissingPunchType MissingPunchType { get; set; }

        public TimeSpan? EntryTime { get; set; }

        public TimeSpan? ExitTime { get; set; }

        public virtual FormRequest FormRequest { get; set; } = null!;
    }
}
