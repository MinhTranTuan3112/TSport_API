using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.Account;

namespace TSport.Api.Services.Interfaces
{
    public interface IAccountService
    {
        Task UpdateCustomerInfo(ClaimsPrincipal claims, UpdateCustomerInfoRequest request);
    }
}