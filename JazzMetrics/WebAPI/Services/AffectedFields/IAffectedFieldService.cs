using Database.DAO;
using Library.Models.AffectedFields;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AffectedFields
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou AffectedField
    /// </summary>
    public interface IAffectedFieldService : ICrudOperations<AffectedFieldModel, AffectedField> { }
}
