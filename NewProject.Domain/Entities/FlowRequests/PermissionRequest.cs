using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultipleFormat.Domain.Entities.FlowRequests
{
    [Table("MFR_PERMISSION_REQUEST")]
    public class PermissionRequest
    {
        public int Id { get; set; }

        public int FormRequestId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int PermissionDays { get; set; }

        public virtual FormRequest FormRequest { get; set; } = null!;
    }
}
