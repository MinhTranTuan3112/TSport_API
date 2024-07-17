using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core;
using Razor.Templating.Core;
using TSport.Api.Models.RequestModels.Email;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmailFactory _fluentEmailFactory;
        private readonly IRazorTemplateEngine _razorTemplateEngine;

        public EmailService(IFluentEmailFactory fluentEmailFactory, IRazorTemplateEngine razorTemplateEngine)
        {
            _fluentEmailFactory = fluentEmailFactory;
            _razorTemplateEngine = razorTemplateEngine;
        }

        public async Task SendOrderConfirmationEmail(Order order)
        {
            var htmlBody = await _razorTemplateEngine.RenderAsync("Views/OrderEmailTemplate.cshtml", order);

            await SendSingleEmail(new SendEmailRequest
            {
                ToEmail = order.CreatedAccount.Email,
                Subject = "[TSport]_Thông tin đơn hàng của bạn",
                Body = htmlBody
            });
        }

        public async Task SendSingleEmail(SendEmailRequest request, bool hasHtmlBody = true)
        {
            await _fluentEmailFactory.Create()
                .To(request.ToEmail)
                .Subject(request.Subject)
                .Body(request.Body, hasHtmlBody)
                .SendAsync();
        }
    }
}