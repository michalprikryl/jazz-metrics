using Database.DAO;
using WebAPI.Models.AspiceVersions;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AspiceVersions
{
    public interface IAspiceVersionService : ICrudOperations<AspiceVersionModel , AspiceVersion> { }
}
