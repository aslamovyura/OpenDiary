using System;
namespace WebUI.ViewModels
{
    public class TopicViewModel
    {
        /// <summary>
        /// Topic identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Topic main content.
        /// </summary>
        public string Text { get; set; }
    }
}