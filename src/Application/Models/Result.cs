using System.Collections.Generic;
using System.Linq;

namespace Application.Models
{
    /// <summary>
    /// Class of result.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="succeeded">Success result.</param>
        /// <param name="errors">List of errors.</param>
        internal Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        /// <summary>
        /// Success result.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// List of errors.
        /// </summary>
        public IEnumerable<string> Errors { get; set; }

        /// <summary>
        /// Success result.
        /// </summary>
        /// <returns>Result.</returns>
        public static Result Success()
        {
            return new Result(true, new string[] { });
        }

        /// <summary>
        /// Error result.
        /// </summary>
        /// <param name="errors">Errors list.</param>
        /// <returns>Error result..</returns>
        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
    }
}