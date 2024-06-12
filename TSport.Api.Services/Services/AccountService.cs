using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using TSport.Api.Models.RequestModels.Account;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;
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
    }
}