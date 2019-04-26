using Database.DAO;
using Library.Models.AspiceProcesses;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AspiceProcesses
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou AffectedField
    /// </summary>
    public interface IAspiceProcessService : ICrudOperations<AspiceProcessModel, AspiceProcess> { }
}
