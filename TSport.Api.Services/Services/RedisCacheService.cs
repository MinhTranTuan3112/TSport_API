using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class RedisCacheService<T> : IRedisCacheService<T> where T : class
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<RedisCacheService<T>> _logger;

        public RedisCacheService(IDistributedCache distributedCache, ILogger<RedisCacheService<T>> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task<T?> GetCacheValueAsync(string key)
        {
            var jsonString = await _distributedCache.GetStringAsync(key);

            return jsonString is null ? null : JsonConvert.DeserializeObject<T>(jsonString);
        }

        public async Task<T?> GetOrSetCacheAsync(string cacheKey, Func<Task<T>> fetchFromDataSource,
        DistributedCacheEntryOptions? options = default)
        {
            options ??= new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Set the cache expiration
            };

            var cachedData = await _distributedCache.GetStringAsync(cacheKey);

            if (string.IsNullOrEmpty(cachedData))
            {
                // Cache miss - fetch data from the original data source
                _logger.LogInformation("REDIS CACHE: Cache miss");
                var data = await fetchFromDataSource();
                var serializedData = JsonConvert.SerializeObject(data);

                await _distributedCache.SetStringAsync(cacheKey, serializedData, options);

                return data;
            }

            return JsonConvert.DeserializeObject<T>(cachedData);
        }

        public async Task SetCacheValueAsync(string key, T cacheValue)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            var jsonString = JsonConvert.SerializeObject(cacheValue);

            await _distributedCache.SetStringAsync(key, jsonString, cacheOptions);
        }
    }
}