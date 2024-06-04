using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Repositories.Repositories;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<ITokenService> _tokenService;
        private readonly Lazy<IOrderService> _cartService;



        public ServiceFactory(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _authService = new Lazy<IAuthService>(() => new AuthService(unitOfWork, this));
            _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration));

            _cartService = new Lazy<IOrderService>(() => new OrderService(unitOfWork, this));


        }

        public IAuthService AuthService => _authService.Value;

        public ITokenService TokenService => _tokenService.Value;

        public IOrderService cartService => _cartService.Value;
    }
}   