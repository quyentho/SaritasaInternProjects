using System.Threading.Tasks;
using UnrealEstate.Business.Email.Models;

namespace UnrealEstate.Business.Email.Interface
{
    public interface IEmailSenderService
    {
        /// <summary>
        ///     Send email service.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendEmailAsync(Message message);
    }
}