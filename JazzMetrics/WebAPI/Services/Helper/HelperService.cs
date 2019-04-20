using Database;
using Database.DAO;
using Library;
using Library.Models;
using Library.Models.AppError;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services.AppErrors;
using WebAPI.Services.Email;
using WebAPI.Services.Helpers;
using WebAPI.Services.Settings;

namespace WebAPI.Services.Helper
{
    /// <summary>
    /// staticka trida pro praci s chybama
    /// </summary>
    public class HelperService : BaseDatabase, IHelperService
    {
        private static readonly string ErrorEmailName = "Email";
        private static readonly string ErrorEmailScope = "ErrorEmail";

        private readonly IEmailService _emailService;
        private readonly ISettingService _settingService;
        private readonly IAppErrorService _appErrorService;

        public HelperService(JazzMetricsContext db, IEmailService email, ISettingService setting, IAppErrorService appErrorService) : base(db)
        {
            _emailService = email;
            _settingService = setting;
            _appErrorService = appErrorService;
        }

        /// <summary>
        /// ulozi chybu do db, pokud zapsani do DB selze, zasle mail s chybou na email dany dle web.configu (element 'error-email')
        /// </summary>
        /// <param name="value">prijaty objekt, obsahujici info o chybe</param>
        /// <param name="saveException">opakovany zapis chyby po vyjimce</param>
        /// <returns>objekt s informaci o vysledku</returns>
        public async Task<BaseResponseModel> SaveErrorToDB(AppErrorModel value, int exceptionRound = 1)
        {
            BaseResponseModel model = new BaseResponseModel();

            try
            {
                await _appErrorService.Create(value);
            }
            catch (Exception e)
            {
                model.Success = false;
                model.Message = "Error wasn't processed.";

                if (exceptionRound < 3) //spadne to 3x? tak do emailu a pokud ani to nepujde, tak do souboru
                {
                    await SaveErrorToDB(value, exceptionRound + 1);
                }
                else
                {
                    await _emailService.SendEmail("Error occured at logging to DB",
                        $"Exception with log:{Environment.NewLine}\n{e.ParseException()}{Environment.NewLine}\nOriginal exception (JSON):{Environment.NewLine}\n{JsonConvert.SerializeObject(value)}",
                        await _settingService.GetSettingValue(ErrorEmailScope, ErrorEmailName));
                }
            }

            return model;
        }

        public CurrentUser GetCurrentUser(int userId)
        {
            User user = Database.User.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                return new CurrentUser
                {
                    Id = user.Id,
                    CompanyId = user.CompanyId
                };
            }
            else
            {
                return null;
            }
        }
    }
}
