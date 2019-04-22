using System.Collections.Generic;

namespace WebApp.Models.Setting.Setting
{
    public class SettingListModel : ViewModel
    {
        public List<SettingViewModel> Settings { get; set; }
    }

    public class SettingViewModel
    {
        public int Id { get; set; }
        public string SettingScope { get; set; }
        public string SettingName { get; set; }
        public string Value { get; set; }
    }

    public class SettingUpdateModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
