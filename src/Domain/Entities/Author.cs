using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// Author.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

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
        public Author()
        {
            Posts = new List<Post>();
            Comments = new List<Comment>();
        }
    }
}