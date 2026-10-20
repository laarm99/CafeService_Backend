namespace MultipleFormat.Application.Modules.Supervisors.DTOs
{
    public class SupervisorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? DomainUserName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
    }
}
