using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Account;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Services.Interfaces
{
    public interface IAccountService
    {
        Task UpdateCustomerInfo(ClaimsPrincipal claims, UpdateCustomerInfoRequest request);

        Task<Account> GetAccountBySupabaseId(string supabaseId);

        //Task<List<GetAccountWithOderReponse>> GetAllAccountWithOrderDetailsCustomer();
        Task<GetAccountWithOderReponse> GetAllAccountWithOrderDetailsCustomer();


    }
}