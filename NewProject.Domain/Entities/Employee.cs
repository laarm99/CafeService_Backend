using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultipleFormat.Domain.Entities
{
    [Table("MFR_EMPLOYEES")]
    public class Employee
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("EMPLOYEE_NUMBER")]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Column("DOMAIN_USER_NAME")]
        public string DomainUserName { get; set; } = string.Empty;

        [Column("FULL_NAME")]
        public string FullName { get; set; } = string.Empty;

        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        [Column("DEPARTMENT")]
        public string Department { get; set; } = string.Empty;

        [Column("SHIFT")]
        public string Shift { get; set; } = string.Empty;

        [Column("POSITION")]
        public string Position { get; set; } = string.Empty;

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
    }
}
