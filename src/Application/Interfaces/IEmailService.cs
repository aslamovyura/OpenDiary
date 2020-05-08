using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Send message to client.
        /// </summary>
        /// <param name="email">Email adress.</param>
        /// <param name="subject">Message subject.</param>
        /// <param name="message">Message</param>
        /// <returns></returns>
        Task SendEmailAsync(string email, string subject, string message);
    }
}