using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NewProject.Domain.Entities.IDM
{
    [Table("IDM_ACCOUNT_ROLE")]
    public class IDMAccountRole
    {
        [Column("ACCOUNT_ID")]
        public int AccountId { get; set; }

        [Column("ROLE_ID")]
        public int RoleId { get; set; }

        public IDMAccount Account { get; set; }

        public IDMRole Role { get; set; }
    }
}
