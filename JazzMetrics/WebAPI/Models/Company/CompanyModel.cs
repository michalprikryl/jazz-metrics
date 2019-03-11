using System.Collections.Generic;
using WebAPI.Models.Users;

namespace WebAPI.Models.Company
{
    public class CompanyModel : BaseResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserModel> Users { get; set; }

        public bool Validate
        {
            get => !string.IsNullOrEmpty(Name);
        }
    }
}
