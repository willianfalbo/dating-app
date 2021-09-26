using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DatingApp.Core.Models.HttpClient;
using Flurl.Http;
using Newtonsoft.Json;

namespace DatingApp.Infrastructure.Clients
{
    public class HttpClientBase : IHttpClientBase
    {
        private readonly IFlurlClient _httpClient;
        private readonly bool _ensureSuccess;

        /// <summary>
        /// Initialize the http client wrapper.
        /// </summary>
        /// <param name="baseUrl">Base url.</param>
        /// <param name="defaultHeaders">Default headers for each request.</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <param name="ensureSuccess">If true, it will throw an exception if any http response is not successful.</param>
        public HttpClientBase(
            string baseUrl,
            Dictionary<string, string> defaultHeaders = null,
            int timeout = 30,
            bool ensureSuccess = true
        )
        {
            _httpClient = new FlurlClient(baseUrl)
                .Configure(c => c.Timeout = TimeSpan.FromSeconds(timeout));

            if (defaultHeaders == null)
            {
                defaultHeaders.Add("Content-Type", "application/json");
                defaultHeaders.Add("Content", "application/json");
            }

            foreach (var (key, value) in defaultHeaders)
                _httpClient.Headers.AddOrReplace(key, value);

            _ensureSuccess = ensureSuccess;
        }

        public async Task<HttpClientResponse<TEntity>> GetAsync<TEntity>(string endpoint)
        {
            var response = await _httpClient.Request(endpoint).GetAsync();

            if (_ensureSuccess && !response.ResponseMessage.IsSuccessStatusCode)
                throw new Exception(GetDefaultErrorMessage(endpoint));

            return await FormatResponse<TEntity>(response);
        }

        public async Task<HttpClientResponse<TEntity>> PostAsync<TEntity>(string endpoint, object body)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body));
            var response = await _httpClient.Request(endpoint).PostAsync(content);

            if (_ensureSuccess && !response.ResponseMessage.IsSuccessStatusCode)
                throw new Exception(GetDefaultErrorMessage(endpoint));

            return await FormatResponse<TEntity>(response);
        }

        public async Task<HttpClientResponse<TEntity>> PutAsync<TEntity>(string endpoint, object body)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body));
            var response = await _httpClient.Request(endpoint).PutAsync(content);

            if (_ensureSuccess && !response.ResponseMessage.IsSuccessStatusCode)
                throw new Exception(GetDefaultErrorMessage(endpoint));

            return await FormatResponse<TEntity>(response);
        }

        public async Task<HttpClientResponse<TEntity>> PatchAsync<TEntity>(string endpoint, object body)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body));
            var response = await _httpClient.Request(endpoint).PatchAsync(content);

            if (_ensureSuccess && !response.ResponseMessage.IsSuccessStatusCode)
                throw new Exception(GetDefaultErrorMessage(endpoint));

            return await FormatResponse<TEntity>(response);
        }

        private async Task<HttpClientResponse<TEntity>> FormatResponse<TEntity>(IFlurlResponse response)
        {
            return new HttpClientResponse<TEntity>
            {
                Data = JsonConvert.DeserializeObject<TEntity>(await response.ResponseMessage.Content.ReadAsStringAsync()),
                IsSuccessful = response.ResponseMessage.IsSuccessStatusCode,
                StatusCode = response.ResponseMessage.StatusCode
            };
        }

        private string GetDefaultErrorMessage(string endpoint) =>
            $"The request to the endpoint {endpoint} has failed.";

        public void Dispose()
        {
            // _httpClient.Dispose();
        }
    }
}