using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TSport.Api.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(int accountId, string role);

        string GenerateRefreshToken();

        ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string expiredAccessToken);
    }
}