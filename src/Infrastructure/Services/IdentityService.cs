using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using Infrastructure.Extentions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;

namespace Masny.QRAnimal.Infrastructure.Services
{
    /// <summary>
    /// Service to manage user authentication.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Constructor with parameters..
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="signInManager">Manager of the user sing in.</param>
        public IdentityService(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        /// <inheritdoc />
        public async Task<string> GetUserIdByNameAsync(string userName)
        {
            var user = await _userManager.Users.FirstAsync(u => u.UserName == userName);

            return user.Id;
        }

        /// <inheritdoc />
        public async Task<(Result result, string userId, string token)> CreateUserAsync(string firstName, string lastName, string email, string userName, DateTime birthDate, string password)
        {
            var user = new ApplicationUser
            {
                //FirstName = firstName,
                //LastName = lastName,
                Email = email,
                UserName = userName,
                //BirthDate = birthDate
            };

            var isExist = await _userManager.FindByEmailAsync(email);

            if (isExist != null)
            {
                return (null, null, null);
            }

            IdentityResult result = await _userManager.CreateAsync(user, password);
            var code = string.Empty;

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }

            return (result.ToApplicationResult(), user.Id, code);
        }

        /// <inheritdoc />
        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return Result.Success();
            }

            return await DeleteUserAsync(user);
        }

        /// <inheritdoc />
        public async Task<Result> LoginUserAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

            return result.ToApplicationResult();
        }

        /// <summary>
        /// User sign in.
        /// </summary>
        /// <param name="email">Email adress.</param>
        /// <param name="userName">User name.</param>
        private async Task SignInUserAsync(string email, string userName)
        {
            var user = new ApplicationUser
            {
                Email = email,
                UserName = userName
            };

            await _signInManager.SignInAsync(user, false);
        }

        /// <inheritdoc />
        public async Task LogoutUserAsync()
        {
            // Delete authentification cookies.
            await _signInManager.SignOutAsync();
        }

        /// <inheritdoc />
        public async Task<(bool result, string message)> EmailConfirmCheckerAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                //return (false, ErrorConstants.UserNotFound);
                return (false, "User is not found!");
            }

            var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (isConfirmed)
            {
                return (true, null);
            }

            //return (false, ErrorConstants.UserNotVerifiedEmail);
            return (false, "Email adress is not verified yet!");
        }

        /// <inheritdoc />
        public async Task<(Result result, string message)> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                //return (null, ErrorConstants.UserNotFound);
                return (null, "User is not found!");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                await SignInUserAsync(user.Email, user.UserName);

                //return (result.ToApplicationResult(), CommonConstants.Successfully);
                return (result.ToApplicationResult(), "Success!");
            }

            //return (result.ToApplicationResult(), ErrorConstants.TokenIssues);
            return (result.ToApplicationResult(), "Token issues..");
        }

        /// <inheritdoc />
        public async Task<(bool result, string userId, string userName, string token)> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return (false, null, null, null);
            }

            var result = await _userManager.IsEmailConfirmedAsync(user);

            if (!result)
            {
                return (false, null, null, null);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return (true, user.Id, user.UserName, token);
        }

        /// <inheritdoc />
        public async Task<Result> ResetPassword(string userName, string password, string code)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return null;
            }

            var result = await _userManager.ResetPasswordAsync(user, code, password);

            return result.ToApplicationResult();
        }

        /// <summary>
        /// Detele user.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>Operation result.</returns>
        private async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }
    }
}