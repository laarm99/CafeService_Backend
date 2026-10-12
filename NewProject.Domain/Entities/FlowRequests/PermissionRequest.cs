using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultipleFormat.Domain.Entities.FlowRequests
{
    [Table("MFR_PERMISSION_REQUEST")]
    public class PermissionRequest
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("FORM_REQUEST_ID")]
        public int FormRequestId { get; set; }

        [Column("START_DATE")]
        public DateTime StartDate { get; set; }

        [Column("END_DATE")]
        public DateTime EndDate { get; set; }

        [Column("RETURN_DATE")]
        public DateTime ReturnDate { get; set; }

        [Column("PERMISSION_DAYS")]
        public int PermissionDays { get; set; }

        public virtual FormRequest FormRequest { get; set; } = null!;
    }
}
