using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Application.Behaviors
{
    /// <summary>
    /// Define class to validate DTO with FluentValidation.
    /// </summary>
    /// <typeparam name="TRequest">Request template.</typeparam>
    /// <typeparam name="TResponse">Response template.</typeparam>
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Constructor with parameter.
        /// </summary>
        /// <param name="validators">FluentValidation processors.</param>
        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        /// <summary>
        /// Validation handler.
        /// </summary>
        /// <param name="request">Requset.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="next">Next validation.</param>
        /// <returns></returns>
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}