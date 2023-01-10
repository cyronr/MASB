using MABS.Application.Common;
using MABS.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MABS.Application.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private int statusCode;
        private string title;
        private string type;
        private string message;


        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(BaseException baseEx)
            {
                statusCode = baseEx.StatusCode;
                title = baseEx.Title;
                type = baseEx.GetType().Name.ToString();
                message = baseEx.Message;

                _logger.LogError($"{title}: {baseEx.Message}");
                _logger.LogDebug($"{title}: {baseEx.Message} ({baseEx.DebugMessage})");

                await HandleExceptionAsync(httpContext, baseEx);
            }
            catch (Exception ex)
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                title = "Internal Server Error.";
                type = "Exception";
                message = "Unknown error occured.";

                _logger.LogCritical($"{title}: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            
            var error = new ErrorDetails()
            {
                type = type,
                title = title,
                status = context.Response.StatusCode
            };
            error.errors.Add(message);

            await context.Response.WriteAsync(error.ToString());
        }
    }
}