using InnoShop.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace InnoShop.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public CustomExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<CustomExceptionHandlerMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "!!! An ERROR occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            var problemDetails = exception switch
            {
                BusinessException businessEx => new ProblemDetails
                {
                    Title = "[Business Error]",
                    Detail = businessEx.Message,
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                },
                NotFoundException notFoundEx => new ProblemDetails
                {
                    Title = "[Not Found Error]",
                    Detail = notFoundEx.Message,
                    Status = (int)HttpStatusCode.NotFound,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
                },
                UnauthorizedAccessException authEx => new ProblemDetails
                {
                    Title = "[Access Denied Error]",
                    Detail = authEx.Message,
                    Status = (int)HttpStatusCode.Forbidden,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
                },
                ArgumentException argEx => new ProblemDetails
                {
                    Title = "[Invalid Argument Error]",
                    Detail = argEx.Message,
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                },
                _ => new ProblemDetails
                {
                    Title = "[Internal Server Error]",
                    Detail = _env.IsDevelopment() ? exception.Message : "An error occurred",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                }
            };

            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            if (_env.IsDevelopment())
            {
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            context.Response.StatusCode = problemDetails.Status ?? 500;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}