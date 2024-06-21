using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using TSport.Api.BusinessLogic.Interfaces;
using TSport.Api.BusinessLogic.Services;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.Club;
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
        private readonly Lazy<IFirebaseStorageService> _firebaseStorageService;
        private readonly Lazy<IClubService> _clubService;

        public ServiceFactory(IUnitOfWork unitOfWork, IConfiguration configuration, StorageClient storageClient,
            IRedisCacheService<PagedResultResponse<GetShirtModel>> pagedResultCacheService, 
            IRedisCacheService<PagedResultResponse<GetClubModel>> clubPagedResultCacheService)
        {
            _authService = new Lazy<IAuthService>(() => new AuthService(unitOfWork, this));
            _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration));
            _clubService = new Lazy<IClubService>(() => new ClubService(unitOfWork, this, clubPagedResultCacheService));
            _shirtService = new Lazy<IShirtService>(() => new ShirtService(unitOfWork, this, pagedResultCacheService));
            _accountService = new Lazy<IAccountService>(() => new AccountService(unitOfWork));
            _firebaseStorageService = new Lazy<IFirebaseStorageService>(() => new FirebaseStorageService(storageClient, configuration));
        }

        public IAuthService AuthService => _authService.Value;

        public IShirtService ShirtService => _shirtService.Value;

        public ITokenService TokenService => _tokenService.Value;

        public IAccountService AccountService => _accountService.Value;
        public IClubService ClubService => _clubService.Value;

        public IFirebaseStorageService FirebaseStorageService => _firebaseStorageService.Value;
    }
}