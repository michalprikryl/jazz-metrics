using WebApp.Models.User;

namespace WebApp.Services.User
{
    public interface IUserManager
    {
        void OnLogin(UserModel user);
        void OnLogout(string email);
        void OnLogout(UserModel user);
        UserModel GetUser(string email);
    }
}
