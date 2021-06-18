using System;
using System.Net;

namespace DatingApp.Core.Models.HttpClient
{
    public class HttpClientResponse<TEntity>
    {
        public TEntity Data { get; set; }
        public bool IsSuccessful { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}