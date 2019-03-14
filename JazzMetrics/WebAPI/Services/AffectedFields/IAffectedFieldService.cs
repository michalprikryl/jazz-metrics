using Database.DAO;
using Library.Models.AffectedFields;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AffectedFields
{
    public interface IAffectedFieldService : ICrudOperations<AffectedFieldModel, AffectedField> { }
}
