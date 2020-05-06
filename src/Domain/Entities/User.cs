using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    /// <summary>
    /// Define application user.
    /// </summary>
    public class User : IdentityUser
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
        /// User's birth date.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// The collection of posts.
        /// </summary>
        public virtual ICollection<Post> Posts { get; set; }

        /// <summary>
        /// User comments.
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public User()
        {
            Posts = new List<Post>();
            Comments = new List<Comment>();
        }
    }
}