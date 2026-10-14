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
        [Column("ID")]
        public int Id { get; set; }

        [Column("FORM_REQUEST_ID")]
        public int FormRequestId { get; set; }

        [Column("ADJUSTMENT_DATE")]
        public DateTime AdjustmentDate { get; set; }

        [Column("MISSING_PUNCH_TYPE")]
        public MissingPunchType MissingPunchType { get; set; }

        [Column("ENTRY_TIME")]
        public TimeSpan? EntryTime { get; set; }

        [Column("EXIT_TIME")]
        public TimeSpan? ExitTime { get; set; }

        [Column("JUSTIFICATION")]
        public string Justification { get; set; } = string.Empty;

        public virtual FormRequest FormRequest { get; set; } = null!;
    }
}
