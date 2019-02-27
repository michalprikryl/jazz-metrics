using System.Threading.Tasks;

namespace WebAPI.Services.Setting
{
    public interface ISettingService
    {
        Task<string> GetSettingValueForEmail(string name);
        Task<string> GetSettingValue(string scope, string name);
    }
}
