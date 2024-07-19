using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.Payment;

namespace TSport.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task AddtoPayment(PaymentResponseModel paymentResponseModel);

    }
}
