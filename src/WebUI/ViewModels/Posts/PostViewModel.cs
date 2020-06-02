using System;
using System.Collections.Generic;

namespace WebUI.ViewModels.Posts
{
    /// <summary>
    /// View model for post object.
    /// </summary>
    public class PostViewModel
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
        /// Post text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Topic identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Post topic.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Post publication date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Author Identifier.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Author name.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Author avatar.
        /// </summary>
        public byte[] AuthorAvatar { get; set; } = null;

        /// <summary>
        /// Current reader Id.
        /// </summary>
        public int CurrentReaderId { get; set; }

        /// <summary>
        /// Collection of post comments.
        /// </summary>
        public ICollection<CommentViewModel> Comments { get; set; }

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public PostViewModel()
        {
            Comments = new List<CommentViewModel>();
        }
    }
}