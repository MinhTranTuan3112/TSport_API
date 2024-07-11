using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StackExchange.Redis;
using TSport.Api.Models.RequestModels.Account;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Enums;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Account> GetAccountBySupabaseId(string supabaseId)
        {
            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);

            if (account is null)
            {
                throw new ForbiddenMethodException("Account not found");                
            }

            return account;
        }

        /* public async Task<List<GetAccountWithOderReponse>> GetAllAccountWithOrderDetailsCustomer()
         {
             var getAllCustomer = await _unitOfWork.AccountRepository.getAllAcountCustomer();

             var responseList = new List<GetAccountWithOderReponse>();

             foreach (var customer in getAllCustomer)
             {
                 var orderDetails = await _unitOfWork.OrderRepository.GetCustomerCartInfo(customer.Id);
                 //add vào responseList
                 var accountWithOrderResponse = customer.Adapt<GetAccountWithOderReponse>();
                 var accountWithOrderResponse.
             }
             // xong xuoi adapt vao GetAccountWithOrderReponse



         }*/
        public async Task<GetAccountWithOderReponse> GetAllAccountWithOrderDetailsCustomer()
        {
            var getAllCustomers = await _unitOfWork.AccountRepository.getAllAcountCustomer();
            var response = new GetAccountWithOderReponse
            {
                Customers = new List<CustomerResponseModel>()
            };
            foreach (var customer in getAllCustomers)
            { 
                //var customerResponseModel =  customer.Adapt<CustomerResponseModel>;
                var customerResponseModel = customer.Adapt<CustomerResponseModel>();

                var orders = await _unitOfWork.OrderRepository.GetCustomerCartInfo(customer.Id);

                if (orders != null)
                {
                    var orderResponseModel = orders.Adapt<OrderResponseModel>();
                    customerResponseModel.Orders.Add(orderResponseModel);
                    response.Customers.Add(customerResponseModel);

                }

            }

            return response;
        }

        public async Task UpdateCustomerInfo(ClaimsPrincipal claims, UpdateCustomerInfoRequest request)
        {
            var accountId = claims.FindFirst(c => c.Type == "aid")?.Value;

            if (accountId is null)
            {
                throw new UnauthorizedException("Unauthorized ");
            }

            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.Id == Convert.ToInt32(accountId));

            if (account is null)
            {
                throw new UnauthorizedException("Account not found");
            }

            request.Adapt(account);
            
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<CustomerResponseModel> ViewMyInfo(ClaimsPrincipal claims)
        {
            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var customer = await _unitOfWork.AccountRepository.FindOneAsync(a => a.SupabaseId == supabaseId);
            if (customer is null)
            {
                throw new BadRequestException("Customer does not exist.");
            }
            var response = customer.Adapt<CustomerResponseModel>();

            var orders = await _unitOfWork.OrderRepository.GetCustomerInfo(response.Id);

            if (orders != null && orders.Count != 0)
            {
                var orderResponseModel = new OrderResponseModel();
                foreach ( var order in orders)
                {
                    orderResponseModel = order.Adapt<OrderResponseModel>();
                    response.Orders.Add(orderResponseModel);
                }
            }

            return response;
        }

    }
}