using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Supabase;
using TSport.Api.Models.RequestModels.Account;
using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.RequestModels.Player;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Repositories.Entities;
using TSport.Api.Services.Interfaces;
using TSport.Api.Services.Services;

namespace TSport.Api.Services.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMapsterConfigurations();
            services.AddSupabaseServiceConfiguartions(configuration);
            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IShirtService, ShirtService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddSingleton(opt => StorageClient.Create());
            services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
            services.AddScoped(typeof(IRedisCacheService<>), typeof(RedisCacheService<>));
            services.AddScoped<ISeasonService, SeasonService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<ISupabaseStorageService, SupabaseStorageService>();
            return services;
        }

        private static IServiceCollection AddSupabaseServiceConfiguartions(this IServiceCollection services, IConfiguration configuration)
        {
            string url = configuration["Supabase:Url"]!;
            string key = configuration["Supabase:APIKey"]!;

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true,
                // SessionHandler = new SupabaseSessionHandler() <-- This must be implemented by the developer
            };

            services.AddSingleton(provider => new Supabase.Client(url, key, options));
            return services;
        }

        private static void AddMapsterConfigurations(this IServiceCollection services)
        {
            TypeAdapterConfig<UpdateCustomerInfoRequest, Account>.NewConfig().IgnoreNullValues(true);
            TypeAdapterConfig<UpdateShirtRequest, Shirt>.NewConfig().IgnoreNullValues(true);
            TypeAdapterConfig<UpdateClubRequest, Club>.NewConfig().IgnoreNullValues(true);
            TypeAdapterConfig<UpdatePlayerRequest, Player>.NewConfig().IgnoreNullValues(true);
            TypeAdapterConfig<UpdateSeasonRequest, Season>.NewConfig().IgnoreNullValues(true);
        }
    }
}