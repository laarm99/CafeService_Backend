using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NewProject.Domain.Entities.IDM
{
    [Table("IDM_ACCOUNT")]
    public class IDMAccount
    {
        [Column("ID")]
        public int Id { get; set; }
        
        [Column("DOMAIN_USER_NAME")]
        public string DomainUserName { get; set; }
        
        [Column("FULL_NAME")]
        public string FullName { get; set; }
        
        [Column("EMAIL")]
        public string Email { get; set; }
        
        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }

        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; }
    }
}
