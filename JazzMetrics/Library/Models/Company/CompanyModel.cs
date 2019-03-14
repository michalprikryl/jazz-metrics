using Library.Models.Users;
using System.Collections.Generic;

namespace Library.Models.Company
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserModel> Users { get; set; }

        public bool Validate() => !string.IsNullOrEmpty(Name);
    }
}
