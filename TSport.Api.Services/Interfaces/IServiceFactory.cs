using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Services.Interfaces
{
    public interface IServiceFactory
    {
        IAuthService AuthService { get; }

        ITokenService TokenService { get; }
        
        ICartService cartService { get; }
    }
}