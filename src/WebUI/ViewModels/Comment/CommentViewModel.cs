using System;
using System.ComponentModel.DataAnnotations;
using Application.Enums;

namespace WebUI.ViewModels.Comment
{
    /// <summary>
    /// View model for comment object.
    /// </summary>
    public class CommentViewModel
    {
        /// <summary>
        /// Comment identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Comment main content.
        /// </summary>
        [Required]
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

        /// <summary>
        /// Comment author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Comment Age [ceil].
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Units of comment age.
        /// </summary>
        public AgeUnits AgeUnits { get; set; }
    }
}