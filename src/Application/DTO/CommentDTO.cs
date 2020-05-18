using System;
namespace Application.DTO
{
    /// <summary>
    /// Comment data transfer object (DTO).
    /// </summary>
    public class CommentDTO
    {
        /// <summary>
        /// Comment identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Comment main content.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Publication date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Post identifier.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Author identifier.
        /// </summary>
        public int AuthorId { get; set; }

        ///// <summary>
        ///// Comment author.
        ///// </summary>
        //public string Author { get; set; }
    }
}