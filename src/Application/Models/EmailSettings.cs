namespace Application.Models
{
    public class EmailSettings
    {
        /// <summary>
        /// SMTP Server.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// SMTP Server port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Sender email adress.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Password of the sender email.
        /// </summary>
        public string Password { get; set; }
    }
}