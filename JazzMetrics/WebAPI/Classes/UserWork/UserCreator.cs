using WebAPI.Models.User;
using Database;
using WebAPI.Classes.Database;
using System.Data.Entity;
using System.Threading.Tasks;

namespace WebAPI.Classes.UserWork
{
    /// <summary>
    /// vytvari uzivatelsky kontext pro uzivatele
    /// </summary>
    public class UserCreator : BaseDatabase
    {
        /// <summary>
        /// samotna metoda vracejici uzivatele dle informaci z databaze
        /// </summary>
        /// <param name="user">uzivatel z databaze</param>
        /// <returns></returns>
        public UserModel CreateUserModel(User user)
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

        public async Task<UserModel> GetUserById(int id)
        {
            using (DatabaseContext = new JazzMetricsEntities())
            {
                User user = await DatabaseContext.Users.FirstOrDefaultAsync(u => u.ID == id);
                if (user != null)
                {
                    return CreateUserModel(user);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}