using Application.Common;
using System.Net;
using System.Text.Json;

namespace Api.Middleware
{
    public class UnauthorizedHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                var response = new OperationResult<string>
                {
                    IsSuccess = false,
                    ErrorMessage = "You must be authenticated and authorized to access this resource.",
                    Data = null
                };

                context.Response.ContentType = "application/json";

                // Prevent double-writing if the body already started
                if (!context.Response.HasStarted)
                {
                    var json = JsonSerializer.Serialize(response);
                    await context.Response.WriteAsync(json);
                }
            }
        }
    }
}