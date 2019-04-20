using Database.DAO;
using Library.Models.AppError;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AppErrors
{
    public interface IAppErrorService : ICrudOperations<AppErrorModel, AppError> { }
}
