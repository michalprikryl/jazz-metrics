using System.Collections.Generic;
using System.Linq;
using WebApp.Models.User;

namespace WebApp.Services.User
{
    public class UserManager : IUserManager
    {
        private readonly List<UserModel> _loggedUsers;

        public UserManager()
        {
            _loggedUsers = new List<UserModel>();
        }

        public void OnLogin(UserModel user) => _loggedUsers.Add(user);

        public void OnLogout(string email) => OnLogout(GetUser(email));

        public void OnLogout(UserModel user) => _loggedUsers.Remove(user);

        public UserModel GetUser(string email) => _loggedUsers.First(u => u.Email == email);
    }
}
