using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Repositories.Repositories;

namespace TSport.Api.Repositories.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoriesDependencies(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IShirtRepository, ShirtRepository>();
            services.AddScoped<ISeasonRepository, SeasonRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<ISeasonPlayerRepository, SeasonPlayerRepository>();
            return services;
        }
    }
}