using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels.Account
{
    /// <summary>
    /// View model for user registration page.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// User first name.
        /// </summary>
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        /// <summary>
        /// User last name.
        /// </summary>
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// User's birth date.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Field {0} requires minimum {2} and maximum {1} symbols!", MinimumLength = 5)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Possword confirmation.
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = "Password mismatch!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}