using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultipleFormat.Domain.Entities
{
    [Table("MFR_SUPERVISORS")]
    public class Supervisor
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("FULL_NAME")]
        public string FullName { get; set; } = string.Empty;

        [Column("NORMALIZED_NAME")]
        public string NormalizedName { get; set; } = string.Empty;

        [Column("DOMAIN_USER_NAME")]
        public string? DomainUserName { get; set; }

        [Column("EMAIL")]
        public string? Email { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;

        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("UPDATED_DATE")]
        public DateTime? UpdatedDate { get; set; }

        [Column("UPDATED_BY")]
        public int? UpdatedBy { get; set; }
    }
}
