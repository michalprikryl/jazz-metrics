using Library.Models;

namespace WebAPI.Services.Helpers
{
    public interface IUser
    {
        CurrentUser CurrentUser { get; set; }
    }
}
