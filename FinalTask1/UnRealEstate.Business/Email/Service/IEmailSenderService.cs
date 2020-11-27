using System.Threading.Tasks;
using UnrealEstate.Business.Email.BussinessModel;

namespace UnrealEstate.Business.Email.Service
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