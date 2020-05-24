using System.Collections.Generic;
using Application.DTO;

namespace WebUI.ViewModels
{
    public class ProfileViewModel
    {
        /// <summary>
        /// User first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's birth date.
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// User age.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Total number of user posts.
        /// </summary>
        public int TotalPostsNumber { get; set; }

        /// <summary>
        /// Total number of number comments.
        /// </summary>
        public int TotalCommentsNumber { get; set; }

        /// <summary>
        /// Latest posts
        /// </summary>
        public ICollection<PostDTO> Posts { get; set; }
    }
}