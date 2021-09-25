using System;
using System.Threading.Tasks;

namespace DatingApp.Core.Interfaces.Services
{
    public interface ICacheService
    {
        /// <summary>
        /// Set value into cache by key. If the key exists, it will be overritten.
        /// </summary>
        /// <param name="key">Unique identifier key.</param>
        /// <param name="value">Value do add in the cache.</param>
        /// <param name="expiration">Expiration for the cache key. If not provided, the default value would be 24h.</param>
        /// <typeparam name="T">Generic class type to serialize.</typeparam>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// Get value in the cache by key.
        /// </summary>
        /// <param name="key">Unique identifier key.</param>
        /// <typeparam name="T">Generic class type to desserialize.</typeparam>
        /// <returns>Cache value</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Remove cache by key.
        /// </summary>
        /// <param name="key">Unique identifier key.</param>
        /// <returns>True for success. Otherwise, false.</returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Remove cache by prefix.
        /// </summary>
        /// <param name="prefix">Unique identifier key.</param>
        /// <returns>Number of removed keys.</returns>
        Task<long> RemoveByPrefixAsync(string prefix);
    }
}
