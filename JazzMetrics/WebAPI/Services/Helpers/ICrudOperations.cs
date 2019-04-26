using Library.Models;
using Library.Networking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Services.Helpers
{
    /// <summary>
    /// interface pro definici metod pro zakladni CRUD operace nad objekty z DB
    /// </summary>
    /// <typeparam name="T">pracovni model</typeparam>
    /// <typeparam name="U">DB trida</typeparam>
    public interface ICrudOperations<T, U>
    {
        /// <summary>
        /// vrati jednu entitu z DB tabulky dle jejiho ID
        /// </summary>
        /// <param name="id">ID entity v DB</param>
        /// <param name="lazy">jestli se maji nacist zavisloti, true -> nenactou se, false -> nactou se</param>
        /// <returns></returns>
        Task<BaseResponseModelGet<T>> Get(int id, bool lazy);
        /// <summary>
        /// vrati vsechny entity z DB tabulky
        /// </summary>
        /// <param name="lazy">jestli se maji nacist zavisloti, true -> nenactou se, false -> nactou se</param>
        /// <returns></returns>
        Task<BaseResponseModelGetAll<T>> GetAll(bool lazy);
        /// <summary>
        /// vytvori entitu v dane tabulce DB
        /// </summary>
        /// <param name="request">model noveho objektu</param>
        /// <returns></returns>
        Task<BaseResponseModelPost> Create(T request);
        /// <summary>
        /// upravy vlastnosti entity z DB
        /// </summary>
        /// <param name="request">upraveny model jiz existujiciho objektu</param>
        /// <returns></returns>
        Task<BaseResponseModel> Edit(T request);
        /// <summary>
        /// smaze entitu z tabulky v DB
        /// </summary>
        /// <param name="id">ID objektu v DB</param>
        /// <returns></returns>
        Task<BaseResponseModel> Drop(int id);
        /// <summary>
        /// upravi nektere vlastnosti entity z DB (specifikovane vlastnosti jsou v implementacich)
        /// <para />
        /// implementovane jen nekde
        /// </summary>
        /// <param name="id">ID entity v DB</param>
        /// <param name="request">upravene vlastnosti entity</param>
        /// <returns></returns>
        Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request);
        /// <summary>
        /// nacte entitu z DB tabulky
        /// </summary>
        /// <param name="id">ID entity v DB</param>
        /// <param name="response">odpoved</param>
        /// <param name="tracking">jestli se ma zpracovani dotazu sledovat, true -> hlidaji se zmeny (vyssi narocnost), false -> pouzije se metoda EF AsNoTracking()</param>
        /// <param name="lazy">jestli se maji nacist zavisloti, true -> nenactou se, false -> nactou se</param>
        /// <returns></returns>
        Task<U> Load(int id, BaseResponseModel response, bool tracking = true, bool lazy = true);
        /// <summary>
        /// prevede DB entitu na pracovni model
        /// </summary>
        /// <param name="dbModel">DB entita</param>
        /// <returns></returns>
        T ConvertToModel(U dbModel);
    }
}
