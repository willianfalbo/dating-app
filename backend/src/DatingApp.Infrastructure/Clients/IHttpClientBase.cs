using System;
using System.Threading.Tasks;
using DatingApp.Core.Models.HttpClient;

namespace DatingApp.Infrastructure.Clients
{
    public interface IHttpClientBase : IDisposable
    {
        Task<HttpClientResponse<TEntity>> GetAsync<TEntity>(string endpoint);
        Task<HttpClientResponse<TEntity>> PostAsync<TEntity>(string endpoint, object body);
        Task<HttpClientResponse<TEntity>> PutAsync<TEntity>(string endpoint, object body);
        Task<HttpClientResponse<TEntity>> PatchAsync<TEntity>(string endpoint, object body);
    }
}