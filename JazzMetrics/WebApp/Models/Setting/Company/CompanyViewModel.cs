using System.Collections.Generic;

namespace WebApp.Models.Setting.Company
{
    public class CompanyViewModel : ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CompanyUser> Users { get; set; }
    }

    public class CompanyUser
    {
        public int UserId { get; set; }
        public bool Admin { get; set; }
        public string Username { get; set; }
        public string UserInfo { get; set; }
    }

    public class CompanyUserModel
    {
        public string Username { get; set; }
        public int CompanyId { get; set; }
    }
}
