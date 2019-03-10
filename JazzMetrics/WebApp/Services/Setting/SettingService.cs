using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models.Setting.AffectedField;
using WebApp.Models.Setting.AspiceProcess;
using WebApp.Models.Setting.AspiceVersion;
using WebApp.Models.Setting.Metric;
using WebApp.Models.Setting.MetricType;
using WebApp.Services.Crud;

namespace WebApp.Services.Setting
{
    public class SettingService : ISettingService
    {
        public static string AffectedFieldEntity => "affectedfield";
        public static string AspiceProcessEntity => "aspiceprocess";
        public static string AspiceVersionEntity => "aspiceversion";
        public static string MetricTypeEntity => "metrictype";
        public static string MetricEntity => "metric";

        private readonly ICrudService _crudService;

        public SettingService(ICrudService crudService) => _crudService = crudService;

        public async Task<List<SelectListItem>> GetAspiceVersionsForSelect(string jwt)
        {
            dynamic response = await _crudService.GetAll<AspiceVersionModel>(jwt, AspiceVersionEntity);

            return ParseServerResponse(response);
        }

        public async Task<List<SelectListItem>> GetAspiceProcessesForSelect(string jwt)
        {
            dynamic response = await _crudService.GetAll<AspiceProcessModel>(jwt, AspiceProcessEntity);

            return ParseServerResponse(response);
        }

        public async Task<List<SelectListItem>> GetMetricTypesForSelect(string jwt)
        {
            dynamic response = await _crudService.GetAll<MetricTypeModel>(jwt, MetricTypeEntity);

            return ParseServerResponse(response);
        }

        public async Task<List<SelectListItem>> GetAffectedFieldsForSelect(string jwt)
        {
            dynamic response = await _crudService.GetAll<AffectedFieldModel>(jwt, AffectedFieldEntity);

            return ParseServerResponse(response);
        }

        public async Task<List<SelectListItem>> GetMetricsForSelect(string jwt)
        {
            dynamic response = await _crudService.GetAll<MetricModel>(jwt, MetricEntity);

            return ParseServerResponse(response);
        }

        private List<SelectListItem> ParseServerResponse(dynamic response)
        {
            if (response.Success)
            {
                List<SelectListItem> values = new List<SelectListItem>();
                foreach (var item in response.Values)
                {
                    values.Add(
                        new SelectListItem
                        {
                            Value = item.Id.ToString(),
                            Text = item.ToString()
                        });
                }
                return values;
            }
            else
            {
                return null;
            }
        }
    }
}
