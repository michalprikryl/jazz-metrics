using System.Threading.Tasks;

namespace WebAPI.Services.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string subject, string text, params string[] to);
    }
}
