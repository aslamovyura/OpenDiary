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
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailTypeRequired")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}