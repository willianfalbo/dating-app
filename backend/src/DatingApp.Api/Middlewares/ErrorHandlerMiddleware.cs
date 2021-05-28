using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DatingApp.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DatingApp.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public ErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger
        )
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// This method is invoked when the error occurs.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                await HandleException(context, ex);
            }
        }

        /// <summary>
        /// This method is used to format the error response.
        /// </summary>
        private static Task HandleException(HttpContext context, Exception ex)
        {
            var errorMessage = ex?.Message;

            var response = context.Response;
            // response.ContentType = "application/json";

            // add response headers
            response.Headers.Add("Application-Error", errorMessage);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            switch (ex)
            {
                case BadRequestException error:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case NotFoundException error:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ConflictException error:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
                case UnauthorizedException error:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // add response body
            // string result = JsonConvert.SerializeObject(new
            // {
            //     status = response.StatusCode,
            //     message = errorMessage,
            //     traceId = context.TraceIdentifier // unique identifier to represent this request in trace logs
            // });

            return response.WriteAsync(errorMessage);
        }
    }
}