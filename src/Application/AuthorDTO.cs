using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.DTO
{
    public class AuthorDTO
    {
        /// <summary>
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }

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
        /// Email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The collection of posts.
        /// </summary>
        public ICollection<Post> Posts { get; set; }

        /// <summary>
        /// User comments.
        /// </summary>
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AuthorDTO()
        {
            Posts = new List<Post>();
            Comments = new List<Comment>();
        }
    }
}