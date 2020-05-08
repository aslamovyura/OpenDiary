using System;
using System.Linq;
using Application.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Extentions
{
    public static class IdentityResultExtensions
    {
        /// <summary>
        /// Transform IdentityResult into the application result.
        /// </summary>
        /// <param name="result">Identity result.</param>
        /// <returns>Application result.</returns>
        public static Result ToApplicationResult(this IdentityResult result)
        {
            result = result ?? throw new ArgumentNullException(nameof(result));

            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }

        /// <summary>
        /// Transform SingInResult to application result.
        /// </summary>
        /// <param name="result">SignIn result.</param>
        /// <returns>Appliation result.</returns>
        public static Result ToApplicationResult(this SignInResult result)
        {
            result = result ?? throw new ArgumentNullException(nameof(result));

            return result.Succeeded
                ? Result.Success()
                : Result.Failure( new string[] { });
        }
    }
}