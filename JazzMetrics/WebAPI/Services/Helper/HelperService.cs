using Database;
using Database.DAO;
using Library.Models;
using Library.Models.Error;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services.Email;
using WebAPI.Services.Helpers;
using WebAPI.Services.Setting;

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

        public HelperService(JazzMetricsContext db, IEmailService email, ISettingService setting) : base(db)
        {
            _emailService = email;
            _settingService = setting;
        }

        /// <summary>
        /// ulozi chybu do db, pokud zapsani do DB selze, zasle mail s chybou na email dany dle web.configu (element 'error-email')
        /// </summary>
        /// <param name="value">prijaty objekt, obsahujici info o chybe</param>
        /// <param name="saveException">opakovany zapis chyby po vyjimce</param>
        /// <returns>objekt s informaci o vysledku</returns>
        public async Task<BaseResponseModel> SaveErrorToDB(ErrorModel value, int exceptionRound = 1)
        {
            BaseResponseModel model = new BaseResponseModel();

            try
            {
                Database.AppError.Add(
                    new AppError
                    {
                        Deleted = false,
                        Exception = value.ExceptionMessage ?? "unknown",
                        Function = value.Function ?? "unknown",
                        InnerException = value.InnerExceptionMessage ?? "unknown",
                        Message = value.Message ?? "unknown",
                        Module = value.Module ?? "unknown",
                        Solved = false,
                        Time = value.Time ?? DateTime.Now,
                        AppInfo = value.User ?? "unknown"
                    });

                await Database.SaveChangesAsync();
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
