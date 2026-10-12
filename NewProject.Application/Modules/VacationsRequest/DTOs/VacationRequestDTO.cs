using MultipleFormat.Domain.Common.Enums;
using System;

namespace MultipleFormat.Application.Modules.VacationsRequest.DTOs
{
    public class VacationRequestDTO
    {
        public int FormRequestId { get; set; }
        public FormType FormType { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string? Comments { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int VacationDays { get; set; }
    }
}
