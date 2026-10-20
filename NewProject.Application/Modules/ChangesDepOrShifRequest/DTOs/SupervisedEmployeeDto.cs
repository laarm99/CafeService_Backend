namespace MultipleFormat.Application.Modules.ChangesDepOrShifRequest.DTOs
{
    public class SupervisedEmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public string DomainUser { get; set; } = string.Empty;
        public string? Plant { get; set; }
        public string? Shift { get; set; }
        public string? Department { get; set; }
        public string? SupervisorName { get; set; }
    }
}
