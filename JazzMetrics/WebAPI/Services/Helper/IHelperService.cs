using Library.Models;
using Library.Models.AppError;
using System.Threading.Tasks;

namespace WebAPI.Services.Helper
{
    /// <summary>
    /// interface pro servis pro praci s chybami
    /// </summary>
    public interface IHelperService
    {
        /// <summary>
        /// nacte uzivatele dle ID
        /// </summary>
        /// <param name="userId">ID uzivatele z DB</param>
        /// <returns></returns>
        CurrentUser GetCurrentUser(int userId);
        /// <summary>
        /// ulozi chybu do db, pokud zapsani do DB selze, zasle mail s chybou na email dany dle DB
        /// </summary>
        /// <param name="value">prijaty objekt, obsahujici info o chybe</param>
        /// <param name="exceptionRound">opakovany zapis chyby po vyjimce</param>
        /// <returns>objekt s informaci o vysledku</returns>
        Task<BaseResponseModel> SaveErrorToDB(AppErrorModel value, int exceptionRound = 1);
    }
}
