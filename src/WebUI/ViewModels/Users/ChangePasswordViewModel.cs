namespace WebUI.ViewModels.Users
{
    /// <summary>
    /// View model for `change password` page.
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User new password.
        /// </summary>
        public string NewPassword { get; set; }
    }
}