using Database.DAO;
using Library.Models.AspiceVersions;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AspiceVersions
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou AffectedField
    /// </summary>
    public interface IAspiceVersionService : ICrudOperations<AspiceVersionModel , AspiceVersion> { }
}
