using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIMFinders.Application.DTOs
{
    public class DocumentUploadDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Details { get; set; }
        public string VIN { get; set; }
        public IFormFile Attachment { get; set; }
    }
}
