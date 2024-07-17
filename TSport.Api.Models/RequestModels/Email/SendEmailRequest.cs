using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.Email
{
    public class SendEmailRequest
    {
        public required string ToEmail { get; set; }
        
        public required string Subject { get; set; }

        public required string Body { get; set; }
    }
}