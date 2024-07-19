using FluentEmail.Core;
using Microsoft.Extensions.Configuration;
using Razor.Templating.Core;
using TSport.Api.BusinessLogic.Interfaces;
using TSport.Api.BusinessLogic.Services;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.Club;
using TSport.Api.Services.BusinessModels.Player;
using TSport.Api.Services.BusinessModels.Shirt;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<IShirtService> _shirtService;
        private readonly Lazy<ITokenService> _tokenService;
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<IClubService> _clubService;
        private readonly Lazy<ISeasonService> _seasonService;
        private readonly Lazy<IPlayerService> _playerService;

        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IOrderDetailsService> _orderdetailsService;
        private readonly Lazy<ISupabaseStorageService> _supabaseStorageService;
        private readonly Lazy<IShirtEditionService> _shirtEditionService;

        private readonly Lazy<IEmailService> _emailService;
        private readonly Lazy<IVnPayService> _vnpayService;
        private readonly Lazy<IPaymentService> _paymentService;


        private readonly Lazy<ISeasonPlayerService> _seasonPlayerService;

        public ServiceFactory(IUnitOfWork unitOfWork, IConfiguration configuration,
            IRedisCacheService<PagedResultResponse<GetShirtInPagingResultModel>> pagedResultCacheService,
            IRedisCacheService<PagedResultResponse<GetClubModel>> clubPagedResultCacheService,
            IRedisCacheService<PagedResultResponse<GetPlayerModel>> playerPagedResultCacheService,
            Supabase.Client client, IFluentEmailFactory fluentEmailFactory, IRazorTemplateEngine razorTemplateEngine)
        {
            _authService = new Lazy<IAuthService>(() => new AuthService(unitOfWork, this));
            _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration));
            _clubService = new Lazy<IClubService>(() => new ClubService(unitOfWork, this, clubPagedResultCacheService));
            _shirtService = new Lazy<IShirtService>(() => new ShirtService(unitOfWork, this, pagedResultCacheService));
            _accountService = new Lazy<IAccountService>(() => new AccountService(unitOfWork));
            _seasonService = new Lazy<ISeasonService>(() => new SeasonService(unitOfWork));
            _playerService = new Lazy<IPlayerService>(() => new PlayerService(unitOfWork, playerPagedResultCacheService));
            _orderService = new Lazy<IOrderService>(() => new OrderService(unitOfWork, this));
            _orderdetailsService = new Lazy<IOrderDetailsService>(() => new OrderDetailsService(unitOfWork));
            _supabaseStorageService = new Lazy<ISupabaseStorageService>(() => new SupabaseStorageService(client, configuration));
            _shirtEditionService = new Lazy<IShirtEditionService>(() => new ShirtEditionService(unitOfWork));
            _emailService = new Lazy<IEmailService>(() => new EmailService(fluentEmailFactory, razorTemplateEngine));
            _vnpayService = new Lazy<IVnPayService>(() => new VnPayService(configuration));
            _paymentService = new Lazy<IPaymentService>(() => new PaymentService(unitOfWork,this));


            _seasonPlayerService = new Lazy<ISeasonPlayerService>(() => new SeasonPlayerService(unitOfWork));
        }

        public IAuthService AuthService => _authService.Value;
        public IShirtService ShirtService => _shirtService.Value;
        public ITokenService TokenService => _tokenService.Value;
        public IAccountService AccountService => _accountService.Value;
        public IClubService ClubService => _clubService.Value;
        public ISeasonService SeasonService => _seasonService.Value;
        public IPlayerService PlayerService => _playerService.Value;
        public IOrderService OrderService => _orderService.Value;
        public IOrderDetailsService OrderDetailsService => _orderdetailsService.Value;
        public ISupabaseStorageService SupabaseStorageService => _supabaseStorageService.Value;
        public IShirtEditionService ShirtEditionService => _shirtEditionService.Value;

        public IEmailService EmailService => _emailService.Value;

        public IVnPayService VnPayService => _vnpayService.Value;
        public ISeasonPlayerService SeasonPlayerService => _seasonPlayerService.Value;

        public IPaymentService PaymentService => _paymentService.Value; 
    }
}