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
using WebAPI.Services.Setting;

namespace WebAPI.Services.Email
{
    public class EmailService : BaseDatabase, IEmailService
    {
        internal static readonly string EmailSettingScope = "EmailSetting";
        private static readonly string EmailSettingSender = "Sender";
        private static readonly string EmailSettingHost = "Host";
        private static readonly string EmailSettingPort = "Port";
        private static readonly string EmailSettingUsername = "Username";
        private static readonly string EmailSettingPassword = "Password";

        private readonly ILogService _logService;
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
