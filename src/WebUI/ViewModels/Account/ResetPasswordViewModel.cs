using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels.Account
{
    /// <summary>
    /// View model for password reset page.
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "PasswordLength", MinimumLength = 5)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Confirm password.
        /// </summary>
        [Required(ErrorMessage = "PasswordConfirmationRequired")]
        [Compare("Password", ErrorMessage = "PasswordMismatch")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Confirmation token.
        /// </summary>
        public string Token { get; set; }
    }
}