using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleFormat.Application.Modules.PermissionsRequest.DTOs
{
    public class AttachmentDto
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[]? Content { get; set; }
    }
}
