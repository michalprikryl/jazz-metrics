using Database;
using Library;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;
using WebAPI.Services.Log;
using WebAPI.Services.Settings;

namespace WebAPI.Services.Email
{
    /// <summary>
    /// servis pro praci s emaily - rozesilani
    /// </summary>
    public class EmailService : BaseDatabase, IEmailService
    {
        /// <summary>
        /// scope v DB tabulce Setting
        /// </summary>
        internal const string EmailSettingScope = "EmailSetting";
        /// <summary>
        /// odesilatel v DB tabulce Setting
        /// </summary>
        private const string EmailSettingSender = "Sender";
        /// <summary>
        /// hostitel mailove schranky v DB tabulce Setting
        /// </summary>
        private const string EmailSettingHost = "Host";
        /// <summary>
        /// port pro email v DB tabulce Setting
        /// </summary>
        private const string EmailSettingPort = "Port";
        /// <summary>
        /// uzivatelske jmeno pro email v DB tabulce Setting
        /// </summary>
        private const string EmailSettingUsername = "Username";
        /// <summary>
        /// heslo pro email v DB tabulce Setting
        /// </summary>
        private const string EmailSettingPassword = "Password";

        /// <summary>
        /// servis pro logovani
        /// </summary>
        private readonly ILogService _logService;
        /// <summary>
        /// servis pro ziskani dat z DB tabulky Setting
        /// </summary>
        private readonly ISettingService _settingService;

        public EmailService(JazzMetricsContext db, ISettingService setting, ILogService log) : base(db)
        {
            _logService = log;
            _settingService = setting;
        }

        public async Task<bool> SendEmail(string subject, string text, params string[] to)
        {
            try
            {
                MimeMessage mail = new MimeMessage
                {
                    Subject = subject,
                    Body = new TextPart(Regex.IsMatch(text, @"<[^>]*>") ? TextFormat.Html : TextFormat.Text) { Text = text },
                };

                mail.From.Add(new MailboxAddress("JazzMetrics", await _settingService.GetSettingValueForEmail(EmailSettingSender)));

                foreach (var item in to)
                {
                    mail.To.Add(new MailboxAddress(item));
                }

                using (SmtpClient client = new SmtpClient())
                {
                    await client.ConnectAsync(await _settingService.GetSettingValueForEmail(EmailSettingHost), int.Parse(await _settingService.GetSettingValueForEmail(EmailSettingPort)));

                    string username = await _settingService.GetSettingValueForEmail(EmailSettingUsername), password = await _settingService.GetSettingValueForEmail(EmailSettingPassword);
                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    {
                        await client.AuthenticateAsync(username, password);
                    }

                    await client.SendAsync(mail);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception e)
            {
                _logService.WriteToFile(LogService.ERROR_LOG, new string[] { text, e.ParseException() });
                
                return false;
            }
        }
    }
}
