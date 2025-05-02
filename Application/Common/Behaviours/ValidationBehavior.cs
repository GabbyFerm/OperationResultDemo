using FluentValidation;
using MediatR;

namespace Application.Common.Behaviours
{
    // MediatR Pipeline Behavior for automatic FluentValidation on requests
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators; // Injects all validators for this request type
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // If there are no validators, skip validation and continue
            if (!_validators.Any())
                return await next();

            // Build validation context for the current request
            var context = new ValidationContext<TRequest>(request);

            // Run all validators in parallel
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // Collect all validation errors
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // If any errors were found, return them in an OperationResult-style response
            if (failures.Any()) 
            { 
                var errors = failures.Select(f => f.ErrorMessage).ToList();

                // Try to find a static method called "Failure(string[])" on TResponse
                var resultType = typeof(TResponse);
                var failureMethod = resultType.GetMethod("Failure", new[] { typeof(string[]) });

                // If such method exists (like OperationResult<T>.Failure), call it dynamically
                if (failureMethod != null)
                    return (TResponse)failureMethod.Invoke(null, new object[] { errors.ToArray() })!;

                // Otherwise, fall back to throwing a FluentValidation exception
                throw new ValidationException(failures);
            }

            // If validation passed, continue to next behavior/handler
            return await next();
        }
    }
}