using System;
using System.Collections.Generic;
using System.Text;
using MultipleFormat.Domain.Common.Enums;

namespace MultipleFormat.Application.Modules.PermissionsRequest.DTOs
{
    public class PermissionCreateRequestDTO
    {
        public int EmployeeId { get; set; }
        
        public PermissionType PermissionType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public int PermissionDays { get; set; }

        public string? SwapDetail { get; set; }

        // Application layer uses AttachmentDto to avoid ASP.NET Core dependency; controller maps IFormFile -> AttachmentDto
        public List<AttachmentDto>? Attachments { get; set; }

        public string? Comments { get; set; }
    }
}
