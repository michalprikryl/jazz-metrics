using System.Threading.Tasks;

namespace WebAPI.Services.Email
{
    /// <summary>
    /// interface pro servis pro praci s emaly
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// odesle mail
        /// </summary>
        /// <param name="subject">predmet mailu</param>
        /// <param name="text">text mailu</param>
        /// <param name="to">prijemci/prijemci mailu</param>
        /// <returns></returns>
        Task<bool> SendEmail(string subject, string text, params string[] to);
    }
}
