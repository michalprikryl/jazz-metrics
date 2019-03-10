using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Setting.AspiceVersion;
using WebApp.Services.Crud;

namespace WebApp.Services.Setting
{
    public class SettingService : ISettingService
    {
        public static string AffectedFieldEntity => "affectedfield";
        public static string AspiceProcessEntity => "aspiceprocess";
        public static string AspiceVersionEntity => "aspiceversion";
        public static string MetricTypeEntity => "metrictype";

        private readonly ICrudService _crudService;

        public SettingService(ICrudService crudService) => _crudService = crudService;

        public async Task<List<SelectListItem>> GetAspiceVersionsForSelect(string jwt)
        {
            var response = await _crudService.GetAll<AspiceVersionModel>(jwt, AspiceVersionEntity);
            if (response.Success)
            {
                return response.Values.Select(v =>
                    new SelectListItem
                    {
                        Value = v.Id.ToString(),
                        Text = v.ToString()
                    }).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
