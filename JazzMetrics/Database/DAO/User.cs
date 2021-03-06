﻿using System.Collections.Generic;

namespace Database.DAO
{
    public partial class User
    {
        public User()
        {
            ProjectUser = new HashSet<ProjectUser>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public int UserRoleId { get; set; }
        public int LanguageId { get; set; }
        public bool UseLdaplogin { get; set; }
        public string LdapUrl { get; set; }
        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual Language Language { get; set; }
        public virtual UserRole UserRole { get; set; }
        public virtual ICollection<ProjectUser> ProjectUser { get; set; }
    }
}
