using System;
using System.Threading.Tasks;
using Application.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    /// <summary>
    /// Interface to manage user authentication.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Get user identifier.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>User id (string).</returns>
        Task<string> GetUserIdByNameAsync(string userName);

        /// <summary>
        /// Get user email by id.
        /// </summary>
        /// <param name="userId">User Identifier.</param>
        /// <returns>User email (string).</returns>
        Task<string> GetEmailByIdAsync(string userId);

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="email">Email adress.</param>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <returns>Operation result, user Id and confirmation token.</returns>
        Task<(Result result, string userId, string token)> CreateUserAsync(string email, string userName, string password);

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Operation result.</returns>
        Task<Result> DeleteUserAsync(string userId);

        /// <summary>
        /// Log in user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="isPersistent">Remember me.</param>
        /// <param name="lockoutOnFailure">Lockout on failure.</param>
        /// <returns>Результат операции.</returns>
        Task<Result> LoginUserAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);

        /// <summary>
        /// Log out from application.
        /// </summary>
        Task LogoutUserAsync();

        /// <summary>
        /// Check whether email is confirmed.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>Result and message.</returns>
        Task<(bool result, string message)> EmailConfirmCheckerAsync(string userName);

        /// <summary>
        /// Confirm user email adress.
        /// </summary>
        /// <param name="userId">User Identifier.</param>
        /// <param name="token">Confirmation Token.</param>
        /// <returns>Operation resutl and token.</returns>
        Task<(Result result, string message)> ConfirmEmail(string userId, string token);

        /// <summary>
        /// Restore user password.
        /// </summary>
        /// <param name="email">Email adress.</param>
        /// <returns>Operation result, user Id, user name and confirmation token.</returns>
        Task<(bool result, string userId, string userName, string token)> ForgotPassword(string email);

        /// <summary>
        /// Reset user password.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="token">Confirmation token.</param>
        /// <returns>Operation result.</returns>
        Task<Result> ResetPassword(string userName, string password, string token);
    }
}