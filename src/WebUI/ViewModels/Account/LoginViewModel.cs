using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels.Account
{
    /// <summary>
    /// View model for user login.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// User email.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Remember 
        /// </summary>
        [Display(Name = "Remember?")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// Return URL after login.
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}