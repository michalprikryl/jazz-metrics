namespace Library.Models.Settings
{
    public class SettingModel
    {
        public int Id { get; set; }
        public string SettingScope { get; set; }
        public string SettingName { get; set; }
        public string Value { get; set; }
    }
}
