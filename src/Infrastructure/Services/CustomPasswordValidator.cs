using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CustomIdentityApp.Services
{
    public class CustomPasswordValidator : IPasswordValidator<User>
    {
        /// <summary>
        /// Minimum required length
        /// </summary>
        public int RequiredLength { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="length"></param>
        public CustomPasswordValidator(int length)
        {
            RequiredLength = length;
        }


        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (string.IsNullOrEmpty(password) || password.Length < RequiredLength)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Minimum password length is {RequiredLength}"
                });
            }
            string pattern = "^[0-9]+$";

            if (!Regex.IsMatch(password, pattern))
            {
                errors.Add(new IdentityError
                {
                    Description = "Password should contain only digits"
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}