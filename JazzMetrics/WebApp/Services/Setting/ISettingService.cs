﻿using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Setting.AffectedField;
using WebApp.Models.Setting.AspiceVersion;
using WebApp.Models.Setting.MetricType;

namespace WebApp.Services.Setting
{
    public interface ISettingService
    {
        Task<BaseApiResultGet<AffectedFieldModel>> GetAllAffectedFields(string jwt);
        Task<AffectedFieldModel> GetAffectedField(int id, string jwt);
        Task<BaseApiResult> CreateAffectedField(AffectedFieldWorkModel model, string jwt);
        Task<BaseApiResult> EditAffectedField(AffectedFieldWorkModel model, string jwt);
        Task<BaseApiResult> DropAffectedField(int id, string jwt);

        Task<BaseApiResultGet<MetricTypeModel>> GetAllMetricTypes(string jwt);
        Task<MetricTypeModel> GetMetricType(int id, string jwt);
        Task<BaseApiResult> CreateMetricType(MetricTypeWorkModel model, string jwt);
        Task<BaseApiResult> EditMetricType(MetricTypeWorkModel model, string jwt);
        Task<BaseApiResult> DropMetricType(int id, string jwt);

        Task<BaseApiResultGet<AspiceVersionModel>> GetAllAspiceVersions(string jwt);
        Task<AspiceVersionModel> GetAspiceVersion(int id, string jwt);
        Task<BaseApiResult> CreateAspiceVersion(AspiceVersionWorkModel model, string jwt);
        Task<BaseApiResult> EditAspiceVersion(AspiceVersionWorkModel model, string jwt);
        Task<BaseApiResult> DropAspiceVersion(int id, string jwt);
    }
}
