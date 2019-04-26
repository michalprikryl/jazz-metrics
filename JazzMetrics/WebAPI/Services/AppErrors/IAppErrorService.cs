using Database.DAO;
using Library.Models.AppError;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AppErrors
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou AppError
    /// </summary>
    public interface IAppErrorService : ICrudOperations<AppErrorModel, AppError> { }
}
