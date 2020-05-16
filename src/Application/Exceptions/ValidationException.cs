using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Exceptions
{
    /// <summary>
    /// Validation error.
    /// </summary>
    public class RequestValidationException : Exception
    {
        private const string _errorMessage = "One or more validation failures have occurred.";

        /// <summary>
        /// Failures dictionary.
        /// </summary>
        public IDictionary<string, string[]> Failures { get; }

        /// <summary>
        /// Define validation error.
        /// </summary>
        public RequestValidationException() : base(_errorMessage)
        {
            Failures = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Define validation error with parameters.
        /// </summary>
        /// <param name="message">Error message.</param>
        public RequestValidationException(string message) : base(message) { }

        /// <summary>
        /// Define validation error with parameters.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public RequestValidationException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Define validation error with parameters.
        /// </summary>
        /// <param name="failures">List of validation failures.</param>
        public RequestValidationException(List<ValidationFailure> failures) : this()
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }
    }
}