using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.BusinessLogic.Interfaces;

namespace TSport.Api.Services.Interfaces
{
    public interface IServiceFactory
    {
        IAuthService AuthService { get; }

        IShirtService ShirtService { get; }

        ITokenService TokenService { get; }

        IAccountService AccountService { get; }

        ISupabaseStorageService SupabaseStorageService { get; }

        IClubService ClubService { get; }

        ISeasonService SeasonService { get; }

        IPlayerService PlayerService { get; }
        IOrderService OrderService { get; }
        IOrderDetailsService OrderDetailsService { get; }
         
        IShirtEditionService ShirtEditionService { get; }

        IEmailService EmailService { get; }
        IVnPayService VnPayService { get; }
        IPaymentService PaymentService { get; }

        ISeasonPlayerService SeasonPlayerService { get; }
    }
}