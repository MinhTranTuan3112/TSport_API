using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Account;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Repositories.Entities;
using TSport.Api.Services.BusinessModels.Account;

namespace TSport.Api.Services.Interfaces
{
    public interface IAccountService
    {
        Task UpdateCustomerInfo(ClaimsPrincipal claims, UpdateCustomerInfoRequest request);

        Task<Account> GetAccountBySupabaseId(string supabaseId);

        Task<GetAccountWithOrderReponse> GetAllAccountWithOrderDetailsCustomer();

        Task<CustomerAccountWithOrderInfoModel> GetDetailsCutomerInfo(ClaimsPrincipal claims);

        Task<GetAccountResponse> GetCustomerInfo(ClaimsPrincipal claims);

    }
}