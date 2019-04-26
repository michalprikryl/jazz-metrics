using Library.Models;

namespace WebAPI.Services.Helpers
{
    /// <summary>
    /// interace zajistujici property s aktualnim uzivatelem
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// aktualne prihlaseny uzivatel
        /// </summary>
        CurrentUser CurrentUser { get; set; }
    }
}
