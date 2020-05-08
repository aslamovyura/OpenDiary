using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels.Account
{
    /// <summary>
    /// View model for the recovery of forgotten password.
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Email adress.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}