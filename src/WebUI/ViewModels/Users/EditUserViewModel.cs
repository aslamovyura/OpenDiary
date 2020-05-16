using System;
namespace WebUI.ViewModels.Users
{
    /// <summary>
    /// View model for `edit user` page.
    /// </summary>
    public class EditUserViewModel
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's birth date.
        /// </summary>
        public DateTime BirthDate { get; set; }
    }
}