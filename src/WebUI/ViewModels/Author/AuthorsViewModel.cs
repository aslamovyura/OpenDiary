using System;
using System.Collections.Generic;

namespace WebUI.ViewModels
{
    /// <summary>
    /// View model for Authors page.
    /// </summary>
    public class AuthorsViewModel
    {
        /// <summary>
        /// User role (admin or user).
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Authors.
        /// </summary>
        public ICollection<AuthorViewModel> Authors { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AuthorsViewModel()
        {
            Authors = new List<AuthorViewModel>();
        }
    }
}