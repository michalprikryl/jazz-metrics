using System.Collections.Generic;

namespace WebApp.Models.Menu
{
    public class MenuViewModel : ViewModel
    {
        public List<MenuItemModel> MenuItems { get; set; }
        public List<string> IdsPath { get; set; }
    }

    public class MenuItemModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ParentID { get; set; }
        public bool Visible { get; set; }
        public bool Last { get; set; }
        public string FunctionID { get; set; }
        public string Parameters { get; set; }
        public bool HasFunction { get { return !string.IsNullOrEmpty(FunctionID); } }
    }
}