using System;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace WebAPI.Classes.Helpers
{
    /// <summary>
    /// staticka trida pro odesilani mailu (defaultne lze nastavit ve webconfigu)
    /// </summary>
    public static class EmailSender
    {
        /// <summary>
        /// samotna metoda pro odesilani emailu
        /// </summary>
        /// <param name="subject">predmet emailu</param>
        /// <param name="text">text emailu</param>
        /// <param name="to">prijemce i prijemci emailu</param>
        /// <returns></returns>
        public static bool SendEmail(string subject, string text, params string[] to)
        {
            try
            {
                MailMessage mail = new MailMessage
                {
                    SubjectEncoding = Encoding.UTF8,
                    BodyEncoding = Encoding.UTF8,
                    Subject = subject,
                    Body = text,
                    IsBodyHtml = Regex.IsMatch(text, @"<[^>]*>"),
                    From = new MailAddress("michal.prikryl.st2@vsb.cz", "Michal")
                };

                foreach (var item in to)
                {
                    mail.To.Add(new MailAddress(item));
                }

                new SmtpClient("smtp.vsb.cz").Send(mail);

                return true;
            }
            catch (Exception e)
            {
                Logger.WriteToFile(Logger.ERROR_LOG, new string[] { text, e.ParseException() });

                return false;
            }
        }
    }
}