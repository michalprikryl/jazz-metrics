using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class Setting
    {
        public int Id { get; set; }
        public string SettingScope { get; set; }
        public string SettingName { get; set; }
        public string Value { get; set; }
    }
}
