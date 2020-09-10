using System.Threading.Tasks;

namespace UnrealEstate.Services.EmailService
{
    public interface IEmailSenderService
    {
        /// <summary>
        /// Send email service.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendEmailAsync(Message message);
    }
}