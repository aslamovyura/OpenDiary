using System;
namespace WebUI.Models
{
    /// <summary>
    /// Post model for API.
    /// </summary>
    public class PostModel
    {
        /// <summary>
        /// Post identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Post title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Post main content.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Publication date.
        /// </summary>
        public DateTime Date { get; set; }

        ///// <summary>
        ///// Author Identifier.
        ///// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Author name.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Topic Identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Topic.
        /// </summary>
        public string Topic { get; set; }
    }
}