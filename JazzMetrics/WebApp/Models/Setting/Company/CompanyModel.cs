using System.Collections.Generic;
using WebApp.Models.User;

namespace WebApp.Models.Setting.Company
{
    public class CompanyModel : BaseApiResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserModel> Users { get; set; }
    }
}
