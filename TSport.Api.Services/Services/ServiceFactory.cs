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
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IOrderDetailsService> _orderdetailsService;




        public ServiceFactory(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _authService = new Lazy<IAuthService>(() => new AuthService(unitOfWork, this));
            _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration));
            _orderService = new Lazy<IOrderService>(() => new OrderService(unitOfWork, this));
            _orderdetailsService = new Lazy<IOrderDetailsService>(() => new OrderDetailsService(unitOfWork, this));

        }

        public IAuthService AuthService => _authService.Value;

        public ITokenService TokenService => _tokenService.Value;

        public IOrderService OrderService => _orderService.Value;

        public IOrderDetailsService OrderDetailsService => _orderdetailsService.Value;
    }
}   