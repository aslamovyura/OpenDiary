using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels.Posts
{
    /// <summary>
    /// View model for post object.
    /// </summary>
    public class CreatePostViewModel
    {
        /// <summary>
        /// Post title.
        /// </summary>
        [Required(ErrorMessage = "TitleRequired")]
        [MaxLength(100, ErrorMessage = "TitleMaxLength")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// Post topic.
        /// </summary>
        [Required(ErrorMessage = "TopicRequired")]
        [MaxLength(30, ErrorMessage = "TopicMaxLength")]
        [Display(Name = "Topic")]
        public string Topic { get; set; }

        /// <summary>
        /// Post text.
        /// </summary>
        [Required(ErrorMessage = "TextRequired")]
        [MinLength(20, ErrorMessage = "TextMinLength")]
        [Display(Name = "Text")]
        public string Text { get; set; }

        /// <summary>
        /// Collection of all topics.
        /// </summary>
        public ICollection<TopicViewModel> Topics { get; set; }

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public CreatePostViewModel()
        {
            Topics = new List<TopicViewModel>();
        }
    }
}