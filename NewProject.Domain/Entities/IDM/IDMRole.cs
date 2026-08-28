using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NewProject.Domain.Entities.IDM
{
    [Table("IDM_ROLE")]
    public class IDMRole
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("ROLE_NAME")]
        public string RoleName { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
    }
}
