using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<ITokenService> _tokenService;
        private readonly Lazy<IShirtService> _shirtService;
        private readonly Lazy<IAccountService> _accountService;

        public ServiceFactory(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _authService = new Lazy<IAuthService>(() => new AuthService(unitOfWork, this));
            _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration));
            _shirtService = new Lazy<IShirtService>(() => new ShirtService(unitOfWork));
            _accountService = new Lazy<IAccountService>(() => new AccountService(unitOfWork));  
        }

        public IAuthService AuthService => _authService.Value;

        public ITokenService TokenService => _tokenService.Value;

        public IShirtService ShirtService => _shirtService.Value;

        public IAccountService AccountService => _accountService.Value;
    }
}