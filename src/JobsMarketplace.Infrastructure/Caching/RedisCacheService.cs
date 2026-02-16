using JobsMarketplace.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace JobsMarketplace.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly TimeSpan _defaultExpiration;

        public RedisCacheService(IDistributedCache cache, IOptions<CacheSettings> options)
        {
            _cache = cache;
            _defaultExpiration = TimeSpan.FromMinutes(options.Value.DefaultExpirationMinutes);
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);
            return value is null
                ? default
                : JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var serialized = JsonSerializer.Serialize(value);

            await _cache.SetStringAsync(
                key,
                serialized,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? _defaultExpiration
                });
        }
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
