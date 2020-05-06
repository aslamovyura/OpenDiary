using System;

namespace WebUI.ViewModels.Posts
{
    public class EditPostViewModel
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
        /// Post topic.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Post publication date.
        /// </summary>
        public DateTime Date { get; set; }
    }
}