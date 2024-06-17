using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using TSport.Api.Models.RequestModels.Account;
using TSport.Api.Repositories.Entities;
using TSport.Api.Services.Interfaces;
using TSport.Api.Services.Services;

namespace TSport.Api.Services.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection services)
        {
            services.AddMapsterConfigurations();
            services.AddHostedServicesDependencies();
            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IShirtService, ShirtService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddSingleton(opt => StorageClient.Create());
            services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
            services.AddSingleton(typeof(IRedisCacheService<>), typeof(RedisCacheService<>));
            return services;
        }

        private static IServiceCollection AddHostedServicesDependencies(this IServiceCollection services)
        {
            services.AddHostedService<CacheRefresherService>();
            return services;
        }

        private static void AddMapsterConfigurations(this IServiceCollection services)
        {
            TypeAdapterConfig<UpdateCustomerInfoRequest, Account>.NewConfig().IgnoreNullValues(true);
        }
    }
}