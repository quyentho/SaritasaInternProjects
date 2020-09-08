using System.Threading.Tasks;

namespace UnrealEstate.Services.EmailService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}