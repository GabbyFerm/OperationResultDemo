using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Common.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        // This method is called for every request passing through the MediatR pipeline
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var stopwatch = Stopwatch.StartNew(); // Start timer to measure execution time

            // Log request start and its payload
            _logger.LogInformation("Handling request: {RequestName} | Payload: {@Request}", requestName, request);

            try
            {
                var response = await next(); // Call the next behavior/handler in the pipeline
                stopwatch.Stop(); // Stop timer after response is returned

                // Log success and how long it took
                _logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms", requestName, stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex) 
            { 
                stopwatch.Stop(); // Stop timer on failure

                // Log the exception and how long it took before the error
                _logger.LogError(ex, "Unhandled exception for request {RequestName} after {ElapsedMilliseconds}ms", requestName, stopwatch.ElapsedMilliseconds);
                throw; // Re-throw so global middleware can handle it
            }
        }
    }
}
