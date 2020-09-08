using System.Threading.Tasks;

namespace UnrealEstate.Services.EmailService
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(Message message);
    }
}