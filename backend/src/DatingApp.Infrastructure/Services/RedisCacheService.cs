using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace DatingApp.Infrastructure.Services
{
    /// <inheritdoc />
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IDatabase _redis;
        private readonly string _cachePrefix;

        /// <summary>
        /// Constructor for redis cache provider.
        /// </summary>
        /// <param name="configuration">Environment configuration.</param>
        public RedisCacheService(IConfiguration configuration)
        {
            var redisConfig = ConfigurationOptions.Parse(configuration["Redis:ConnectionString"], false);
            redisConfig.ResolveDns = true;

            _redisConnection = ConnectionMultiplexer.Connect(redisConfig);

            _redis = _redisConnection.GetDatabase();

            _cachePrefix = configuration["Redis:CachePrefix"] ?? throw new ArgumentNullException("Redis:CachePrefix");
        }

        /// <inheritdoc />
        public Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            // cache expires in
            expiration = expiration ?? TimeSpan.FromHours(24);

            var jsonConfig = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            return _redis.StringSetAsync(
                $"{_cachePrefix}:{key}",
                JsonConvert.SerializeObject(value, jsonConfig),
                expiration
            );
        }

        /// <inheritdoc />
        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var content = await _redis.StringGetAsync($"{_cachePrefix}:{key}");
            if (content.HasValue)
                return JsonConvert.DeserializeObject<T>(content.ToString());

            return default(T);
        }

        /// <inheritdoc />
        public Task<bool> RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return _redis.KeyDeleteAsync($"{_cachePrefix}:{key}");
        }

        /// <inheritdoc />
        public async Task<long> RemoveByPrefixAsync(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentNullException(nameof(prefix));

            long removedKeys = 0;

            foreach (var endpoint in _redisConnection.GetEndPoints())
            {
                var server = _redisConnection.GetServer(endpoint);
                var keys = server.Keys(pattern: $"{_cachePrefix}:{prefix}*").ToArray();
                removedKeys += await _redis.KeyDeleteAsync(keys);
            }

            return removedKeys;
        }
    }
}