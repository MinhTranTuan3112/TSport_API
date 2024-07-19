using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.Payment;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;

        public PaymentService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
        }

        public async Task AddtoPayment(ClaimsPrincipal claims, PaymentResponseModel payment)
        {

            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var supabaseId = "580b1b9e-c395-467c-a4e8-ce48c0ec09d1";
            if (supabaseId is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }

            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId.Equals(supabaseId));
            if (account is null)
            {
                throw new NotFoundException("Account not found");
            }
         //   var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);

            var getCart = await _unitOfWork.OrderRepository.GetCustomerCartInfo(account.Id);
            var getAmmountPayment = await _unitOfWork.PaymentRepository.getAmmountPayment()+1;
            var getStatus = payment.Success;
            var statusReponse ="";
            if (getStatus == true) {
                statusReponse = "Success";
                getCart.Status = "Processed";
                await _unitOfWork.OrderRepository.UpdateAsync(getCart);

            }
            else
            {
                statusReponse = "Fail";

            }
            if (account is null)
            {
                throw new UnauthorizedException("Unauthorized");
            }
            var newPayment = new Payment
            {
                Amount = getCart.Total,
                Code = $"PAY{getAmmountPayment.ToString()}",
                OrderId = getCart.Id,
                PaymentMethod = "VNpay",
                CreatedDate = DateTime.Now,
                Status = statusReponse,
                CreatedAccountId = account.Id,

            };
            await _unitOfWork.PaymentRepository.AddAsync(newPayment);
            await _unitOfWork.SaveChangesAsync();


        }
    }
}
