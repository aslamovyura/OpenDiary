using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels
{
    /// <summary>
    /// View model for post object.
    /// </summary>
    public class CreatePostViewModel
    {
        /// <summary>
        /// Post title.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// Post topic.
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string Topic { get; set; }

        /// <summary>
        /// Post text.
        /// </summary>
        [Required]
        [MinLength(20)]
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