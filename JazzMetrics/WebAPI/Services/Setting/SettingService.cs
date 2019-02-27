using Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebAPI.Services.Email;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Setting
{
    public class SettingService : BaseDatabase, ISettingService
    {
        public SettingService(JazzMetricsContext db) : base(db) { }

        public Task<string> GetSettingValueForEmail(string name)
        {
            return GetSettingValue(EmailService.EmailSettingScope, name);
        }

        public async Task<string> GetSettingValue(string scope, string name)
        {
            return (await Database.Setting.FirstOrDefaultAsync(s => s.SettingScope == scope && s.SettingName == name))?.Value ?? string.Empty;
        }
    }
}
