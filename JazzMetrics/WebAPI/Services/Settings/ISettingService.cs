using Database.DAO;
using Library.Models.Settings;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Settings
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou Setting
    /// </summary>
    public interface ISettingService : ICrudOperations<SettingModel, Setting>
    {
        /// <summary>
        /// nacte vlastnost pro emailove nastaveni
        /// </summary>
        /// <param name="name">nazev nastaveni</param>
        /// <returns></returns>
        Task<string> GetSettingValueForEmail(string name);
        /// <summary>
        /// nacte hodnotu nastaveni
        /// </summary>
        /// <param name="scope">scope nastaveni</param>
        /// <param name="name">nazev nastaveni</param>
        /// <returns></returns>
        Task<string> GetSettingValue(string scope, string name);
    }
}
