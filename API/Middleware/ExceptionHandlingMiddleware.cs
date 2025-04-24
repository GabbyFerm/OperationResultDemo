using Application.Common;
using System.Net;
using System.Text.Json;

namespace Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        // Global error handler middleware: logs and returns safe OperationResult error response (to catch unhandled exceptions)
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unhandled exception occurred.");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new OperationResult<string>
                {
                    IsSuccess = false,
                    ErrorMessage = "An unexpected error occurred. Please try again later.",
                    Data = null
                };

                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }
}

