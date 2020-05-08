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
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Field {0} requires minimum {2} and maximum {1} symbols!", MinimumLength = 5)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Подтвердить пароль.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Confirmation token.
        /// </summary>
        public string Token { get; set; }
    }
}