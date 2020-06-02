using System;
using Microsoft.AspNetCore.Http;

namespace WebUI.ViewModels
{
    /// <summary>
    /// View model for `edit user` page.
    /// </summary>
    public class AuthorViewModel
    {
        /// <summary>
        /// Author identifier.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// User identifier.
        /// </summary>
        public string UserId { get; set; }

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

        /// <summary>
        /// Posts number.
        /// </summary>
        public int PostsNumber { get; set; }

        /// <summary>
        /// Comments number.
        /// </summary>
        public int CommentsNumber { get; set; }

        /// <summary>
        /// Author avatar.
        /// </summary>
        public byte[] Avatar { get; set; } = null;
    }
}