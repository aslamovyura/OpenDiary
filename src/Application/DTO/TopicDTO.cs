using System;
namespace Application.DTO
{
    /// <summary>
    /// Topic Data Transfer Object (DTO).
    /// </summary>
    public class TopicDTO
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