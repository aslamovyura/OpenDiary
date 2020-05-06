using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// Define user topic.
    /// </summary>
    public class Topic
    {
        /// <summary>
        /// Topic identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Topic main content.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Post.
        /// </summary>
        public ICollection<Post> Posts { get; set; }

        /// <summary>
        /// Topic constructor.
        /// </summary>
        public Topic()
        {
            Posts = new List<Post>();
        }
    }
}