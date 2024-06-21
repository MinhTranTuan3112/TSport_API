using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace TSport.Api.Services.Interfaces
{
    public interface IRedisCacheService<T> where T : class
    {
        Task<T?> GetCacheValueAsync(string key);

        Task SetCacheValueAsync(string key, T cacheValue);

        Task<T?> GetOrSetCacheAsync(string cacheKey, Func<Task<T>> fetchFromDataSource, DistributedCacheEntryOptions? options = default);
    }
}