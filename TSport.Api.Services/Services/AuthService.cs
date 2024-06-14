using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using TSport.Api.Models.RequestModels;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Models.ResponseModels.Auth;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;

        public AuthService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
        }

        public async Task<GetAccountResponse> GetAuthAccountInfo(ClaimsPrincipal claims)
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

            return account.Adapt<GetAccountResponse>();
        }

        public async Task<AuthTokensResponse> Login(LoginRequest request)
        {
            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.Email == request.Email
            && a.Password == HashPassword(request.Password));

            if (account is null)
            {
                throw new UnauthorizedException("Wrong email or password");
            }

            string accessToken = _serviceFactory.TokenService.GenerateAccessToken(account.Id, account.Role);
            string refreshToken = _serviceFactory.TokenService.GenerateRefreshToken();

            return new AuthTokensResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            // Convert the password string to bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Compute the hash
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            // Convert the hash to a hexadecimal string
            string hashedPassword = string.Concat(hashBytes.Select(b => $"{b:x2}"));

            return hashedPassword;
        }

        public async Task RegisterAccount(RegisterRequest request)
        {
            var account = await _unitOfWork.AccountRepository.FindOneAsync(a => a.Email == request.Email);

            if (account is not null)
            {
                throw new BadRequestException("Account with this email already exists");
            }
            
            
            var newAccount = request.Adapt<Account>();
            newAccount.Password = HashPassword(request.Password);
            await _unitOfWork.AccountRepository.AddAsync(newAccount);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<GetAccountResponse> GetAuthAccountInfoFromSupabaseClaims(ClaimsPrincipal claims)
        {
            var supabaseId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(supabaseId))
            {
                throw new ForbiddenMethodException("You don't have permission to access this resource");
            }

            var account = await _serviceFactory.AccountService.GetAccountBySupabaseId(supabaseId);

            return account.Adapt<GetAccountResponse>();
        }
    }
}