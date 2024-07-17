using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Email;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendSingleEmail(SendEmailRequest request, bool hasHtmlBody = true);

        Task SendOrderConfirmationEmail(Order order);
    }
}