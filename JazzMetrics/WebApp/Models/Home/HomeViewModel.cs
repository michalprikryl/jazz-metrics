using System.Collections.Generic;

namespace WebApp.Models.Home
{
    public class HomeViewModel : ViewModel
    {
        public List<HomeProjectModel> Projects { get; set; }
    }

    public class HomeProjectModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
    }
}
