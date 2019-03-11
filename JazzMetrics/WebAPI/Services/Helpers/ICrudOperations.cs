﻿using Library.Networking;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services.Helpers
{
    public interface ICrudOperations<T, U>
    {
        Task<T> Get(int id, bool lazy);
        Task<BaseResponseModelGet<T>> GetAll(bool lazy);
        Task<BaseResponseModelPost> Create(T request);
        Task<BaseResponseModel> Edit(T request);
        Task<BaseResponseModel> Drop(int id);
        Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request);
        Task<U> Load(int id, BaseResponseModel response);
        T ConvertToModel(U dbModel);
    }
}
