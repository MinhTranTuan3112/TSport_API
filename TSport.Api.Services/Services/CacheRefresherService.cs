using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Services.BusinessModels.Shirt;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class CacheRefresherService : BackgroundService
    {
        private readonly ILogger<CacheRefresherService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(5);

        public CacheRefresherService(ILogger<CacheRefresherService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BACKGROUND WORKER: Cache refresher start running");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("BACKGROUND WORKER: Refreshing cache");

                    var cacheEntryOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    };

                    using var scope = _serviceProvider.CreateScope();

                    var pagedShirtsCacheService = scope.ServiceProvider.GetRequiredService<IRedisCacheService<PagedResultResponse<GetShirtInPagingResultModel>>>();
                    var serviceFactory = scope.ServiceProvider.GetRequiredService<IServiceFactory>();

                    await SetPagedShirtsCache(cacheEntryOptions, pagedShirtsCacheService, serviceFactory);

                    await Task.Delay(_refreshInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "BACKGROUND WORKER: An error occurred while refreshing cache");
                }
            }

            _logger.LogInformation("BACKGROUND WORKER: Cache refresher is stopping");
        }

        private async Task SetPagedShirtsCache(DistributedCacheEntryOptions cacheEntryOptions, IRedisCacheService<PagedResultResponse<GetShirtInPagingResultModel>> pagedShirtsCacheService, IServiceFactory serviceFactory)
        {
            _logger.LogInformation("BACKGROUND WORKER: Attempting to set cache for paged shirts");
            var pagedShirts = await serviceFactory.ShirtService.GetCachedPagedShirts(new QueryPagedShirtsRequest());
            _logger.LogInformation($"BACKGROUND WORKER: Retrieved {pagedShirts.Items.Count} results for caching");
            await pagedShirtsCacheService.GetOrSetCacheAsync(
                "pagedShirts",
                () => Task.FromResult(pagedShirts),
                cacheEntryOptions
            );
            _logger.LogInformation("BACKGROUND WORKER: Cache set successfully");
        }
    }
}