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
        [Required(ErrorMessage = "FirstNameRequired")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// User last name.
        /// </summary>
        [Required(ErrorMessage = "LastNameRequired")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailTypeRequired")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// User's birth date.
        /// </summary>
        [Required(ErrorMessage = "BirthDateRequired")]
        [DataType(DataType.Date, ErrorMessage = "DateTypeRequired")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "BirthDate")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "PasswordLength", MinimumLength = 5)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Possword confirmation.
        /// </summary>
        [Required(ErrorMessage = "PasswordConfirmationRequired")]
        [Compare("Password", ErrorMessage = "PasswordMismatch")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        public string PasswordConfirm { get; set; }
    }
}