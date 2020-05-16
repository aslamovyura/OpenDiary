using System;

namespace Application.Exceptions
{
    /// <summary>
    /// Error about the lack of entity.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Define exception.
        /// </summary>
        public NotFoundException() { }

        /// <summary>
        /// Define exception with parameters.
        /// </summary>
        /// <param name="message">Message.</param>
        public NotFoundException(string message) : base(message) { }

        /// <summary>
        /// Define exception with parameters.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner error.</param>
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Define exception with parameters.
        /// </summary>
        /// <param name="name">Entity name.</param>
        /// <param name="key">Key.</param>
        public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.") { }
    }
}
