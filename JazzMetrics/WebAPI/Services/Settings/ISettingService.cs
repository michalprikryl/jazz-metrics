using Database.DAO;
using Library.Models.Settings;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Settings
{
    public interface ISettingService : ICrudOperations<SettingModel, Setting>
    {
        Task<string> GetSettingValueForEmail(string name);
        Task<string> GetSettingValue(string scope, string name);
    }
}
