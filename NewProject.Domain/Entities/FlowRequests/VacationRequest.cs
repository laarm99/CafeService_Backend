using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultipleFormat.Domain.Entities.FlowRequests
{
    [Table("MFR_VACATION_REQUEST")]
    public class VacationRequest
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

        [Column("VACATION_DAYS")]
        public int VacationDays { get; set; }

        public virtual FormRequest FormRequest { get; set; } = null!;
    }
}
