using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services.Helpers
{
    public interface ICrudOperations<T, U>
    {
        Task<T> Get(int id);
        Task<BaseResponseModelGet<T>> GetAll();
        Task<BaseResponseModel> Create(T request);
        Task<BaseResponseModel> Edit(T request);
        Task<BaseResponseModel> Drop(int id);
        Task<U> Load(int id, BaseResponseModel response);
        T ConvertToModel(U dbModel);
    }
}
