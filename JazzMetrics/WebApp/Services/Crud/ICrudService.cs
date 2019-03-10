using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services.Crud
{
    public interface ICrudService
    {
        Task<BaseApiResultGet<T>> GetAll<T>(string jwt, string entity, bool lazy = true);
        Task<T> Get<T>(int id, string jwt, string entity, bool lazy = true);
        Task<BaseApiResultPost> Create<T>(T model, string jwt, string entity);
        Task<BaseApiResult> Edit<T>(int id, T model, string jwt, string entity);
        Task<BaseApiResult> Drop(int id, string jwt, string entity);
    }
}
