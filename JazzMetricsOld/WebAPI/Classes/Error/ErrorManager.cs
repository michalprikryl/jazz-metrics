using Newtonsoft.Json;
using WebAPI.Classes.Helpers;
using WebAPI.Models;
using WebAPI.Models.Error;
using System;
using System.Configuration;
using System.Threading.Tasks;
using Database;

namespace WebAPI.Classes.Error
{
    /// <summary>
    /// staticka trida pro praci s chybama
    /// </summary>
    public static class ErrorManager
    {
        /// <summary>
        /// ulozi chybu do db, pokud zapsani do DB selze, zasle mail s chybou na email dany dle web.configu (element 'error-email')
        /// </summary>
        /// <param name="value">prijaty objekt, obsahujici info o chybe</param>
        /// <param name="saveException">opakovany zapis chyby po vyjimce</param>
        /// <returns>objekt s informaci o vysledku</returns>
        public static async Task<BaseResponseModel> SaveErrorToDB(ErrorModel value, bool saveException = true)
        {
            BaseResponseModel model = new BaseResponseModel();

            using (JazzMetricsEntities db = new JazzMetricsEntities())
            {
                try
                {
                    db.AppErrors.Add(
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

                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    model.Success = false;
                    model.Message = "Chyba nebyla uložena.";

                    if (saveException)
                    {
                        SaveErrorObject(value);
                        await SaveErrorToDB(new ErrorModel(e, message: "input object->file!", module: "ErrorManager", function: "SaveError"), false);
                    }
                    else
                    {
                        EmailSender.SendEmail("Chyba při ukládání erroru v API", e.ParseException(), ConfigurationManager.AppSettings["error-email"]);
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// ulozi objekt s informacema o chybe do souboru
        /// </summary>
        /// <param name="model"></param>
        private static void SaveErrorObject(ErrorModel model)
        {
            Logger.WriteToFile($"error_{DateTime.Now.GetDateTimeString().Replace(' ', '_').Replace(':', '-')}.txt", JsonConvert.SerializeObject(model));
        }
    }
}