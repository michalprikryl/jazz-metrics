using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace WebAPI.Classes.Setting
{
    /// <summary>
    /// trida pro nacitani nastaveni scanneru ze souboru
    /// </summary>
    public class Settings
    {
        private const string _settingFileName = "settings.json";

        private readonly string _file;

        /// <summary>
        /// objekt, obsahujici kompletni nastaveni
        /// </summary>
        public SettingModel Setting { get; private set; }

        public Settings()
        {
            _file = $"{Extensions.PATH}{_settingFileName}";
        }

        /// <summary>
        /// nacte nastaveni do tridni property Setting
        /// </summary>
        public void LoadSettings()
        {
            FillSettings(string.Join("", File.ReadAllLines(_file)));
        }

        /// <summary>
        /// nacte asynchronne nastaveni do tridni property Setting
        /// </summary>
        /// <returns></returns>
        public async Task LoadSettingsAsync()
        {
            using (StreamReader reader = File.OpenText(_file))
            {
                FillSettings(await reader.ReadToEndAsync());
            }
        }

        private void FillSettings(string settingsString)
        {
            Setting = JsonConvert.DeserializeObject<SettingModel>(settingsString);
        }
    }
}