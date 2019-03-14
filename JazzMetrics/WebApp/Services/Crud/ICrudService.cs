using Library.Models;
using Library.Networking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Services.Crud
{
    public interface ICrudService
    {
        Task<BaseResponseModelGetAll<T>> GetAll<T>(string jwt, string entity, bool lazy = true);
        Task<BaseResponseModelGet<T>> Get<T>(int id, string jwt, string entity, bool lazy = true);
        Task<BaseResponseModelPost> Create<T>(T model, string jwt, string entity);
        Task<BaseResponseModel> Edit<T>(int id, T model, string jwt, string entity);
        Task<BaseResponseModel> Drop(int id, string jwt, string entity);
        Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> model, string jwt, string entity);
    }
}
