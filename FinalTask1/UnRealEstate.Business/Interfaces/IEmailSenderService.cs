using System.Threading.Tasks;
using UnrealEstate.Business.EmailService;

namespace UnrealEstate.Business.Interfaces
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