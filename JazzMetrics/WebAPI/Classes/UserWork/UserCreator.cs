using WebAPI.Models.User;
using Database;

namespace WebAPI.Classes.UserWork
{
    /// <summary>
    /// vytvari uzivatelsky kontext pro uzivatele
    /// </summary>
    public static class UserCreator
    {
        /// <summary>
        /// samotna metoda vracejici uzivatele dle informaci z databaze
        /// </summary>
        /// <param name="user">uzivatel z databaze</param>
        /// <returns></returns>
        public static UserModel CreateUserModel(User user)
        {
            return new UserModel
            {
                UserId = user.ID,
                FirstName = user.Name,
                Lastname = user.LastName,
                Email = user.Email,
                Role = user.Role.Name
            };
        }
    }
}