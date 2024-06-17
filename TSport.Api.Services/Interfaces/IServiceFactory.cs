using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Services.Interfaces
{
    public interface IServiceFactory
    {
        IAuthService AuthService { get; }

        IShirtService ShirtService { get; }

        ITokenService TokenService { get; }

        IAccountService AccountService { get; }

        IFirebaseStorageService FirebaseStorageService { get; }

        IOrderService OrderService { get; }
        IOrderDetailsService OrderDetailsService { get; }
    }
}