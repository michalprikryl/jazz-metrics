using Database;
using Database.DAO;
using Library;
using Library.Models;
using Library.Models.AppError;
using Microsoft.EntityFrameworkCore;
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
    /// servis pro praci s chybami
    /// </summary>
    public class HelperService : BaseDatabase, IHelperService
    {
        /// <summary>
        /// nazev nastaveni v DB tabulce Setting
        /// </summary>
        private static readonly string ErrorEmailName = "Email";
        /// <summary>
        /// scope nastaveni v tabulce Setting
        /// </summary>
        private static readonly string ErrorEmailScope = "ErrorEmail";

        /// <summary>
        /// servis pro praci s emaily
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// servis pro praci s entitou Setting
        /// </summary>
        private readonly ISettingService _settingService;
        /// <summary>
        /// servis pro praci s entitou AppError
        /// </summary>
        private readonly IAppErrorService _appErrorService;

        /// <summary>
        /// aktualne prihlaseny uzivatel (dle JWT)
        /// </summary>
        private CurrentUser _currentUser;

        public HelperService(JazzMetricsContext db, IEmailService email, ISettingService setting, IAppErrorService appErrorService) : base(db)
        {
            _emailService = email;
            _settingService = setting;
            _appErrorService = appErrorService;
        }

        /// <summary>
        /// ulozi chybu do db, pokud zapsani do DB selze, zasle mail s chybou na email dany dle DB
        /// </summary>
        /// <param name="value">prijaty objekt, obsahujici info o chybe</param>
        /// <param name="exceptionRound">opakovany zapis chyby po vyjimce</param>
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

        /// <summary>
        /// nacte uzivatele dle ID
        /// </summary>
        /// <param name="userId">ID uzivatele z DB</param>
        /// <returns></returns>
        public CurrentUser GetCurrentUser(int userId)
        {
            if (_currentUser == null)
            {
                User user = Database.User.Include(u => u.UserRole).FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    _currentUser = new CurrentUser
                    {
                        Id = user.Id,
                        CompanyId = user.CompanyId,
                        RoleName = user.UserRole.Name
                    };
                }
                else
                {
                    return null;
                }
            }

            return _currentUser;
        }
    }
}
