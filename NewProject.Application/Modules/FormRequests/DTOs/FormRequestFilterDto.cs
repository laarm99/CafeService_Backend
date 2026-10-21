using MultipleFormat.Domain.Common.Enums;

namespace MultipleFormat.Application.Modules.FormRequests.DTOs
{
    public class FormRequestFilterDto
    {
        public FormType? FormType { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
