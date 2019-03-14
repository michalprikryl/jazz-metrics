using Database.DAO;
using Library.Models.AspiceProcesses;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AspiceProcesses
{
    public interface IAspiceProcessService : ICrudOperations<AspiceProcessModel, AspiceProcess> { }
}
