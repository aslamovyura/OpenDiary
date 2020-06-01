namespace WebUI.ViewModels
{
    /// <summary>
    /// View model to use in the email newsletter.
    /// </summary>
    public class EmailViewModel
    {
        /// <summary>
        /// Author name.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Callback URL.
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// URL of the home page.
        /// </summary>
        public string HomePageUrl { get; set; }

        /// <summary>
        /// Logo image URL.
        /// </summary>
        public string LogoUrl { get; set; }
    }
}