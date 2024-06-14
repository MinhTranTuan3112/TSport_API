using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Models.ResponseModels.Auth;

namespace TSport.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthTokensResponse> Login(LoginRequest request);
        Task<GetAccountResponse> GetAuthAccountInfo(ClaimsPrincipal claims);
        Task<GetAccountResponse> GetAuthAccountInfoFromSupabaseClaims(ClaimsPrincipal claims);
        Task RegisterAccount(RegisterRequest request);    
    }
}