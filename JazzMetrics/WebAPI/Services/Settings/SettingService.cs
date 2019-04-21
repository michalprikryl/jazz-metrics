﻿using Database;
using Database.DAO;
using Library.Models;
using Library.Models.Settings;
using Library.Networking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Services.Email;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Settings
{
    public class SettingService : BaseDatabase, ISettingService
    {
        public SettingService(JazzMetricsContext db) : base(db) { }

        public Task<string> GetSettingValueForEmail(string name)
        {
            return GetSettingValue(EmailService.EmailSettingScope, name);
        }

        public async Task<string> GetSettingValue(string scope, string name)
        {
            return (await Database.Setting.FirstOrDefaultAsync(s => s.SettingScope == scope && s.SettingName == name))?.Value ?? string.Empty;
        }

        public Task<BaseResponseModelGet<SettingModel>> Get(int id, bool lazy)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponseModelGetAll<SettingModel>> GetAll(bool lazy)
        {
            var response = new BaseResponseModelGetAll<SettingModel> { Values = new List<SettingModel>() };

            foreach (var item in await Database.Setting.ToListAsync())
            {
                response.Values.Add(ConvertToModel(item));
            }

            return response;
        }

        public Task<BaseResponseModelPost> Create(SettingModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModel> Edit(SettingModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModel> Drop(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            BaseResponseModel response = new BaseResponseModel();

            Setting setting = await Load(id, response);
            if (setting != null)
            {
                foreach (var item in request)
                {
                    if (string.Compare(item.PropertyName, "value", true) == 0)
                    {
                        setting.Value = item.Value;
                    }
                }

                await Database.SaveChangesAsync();

                response.Message = "Setting was successfully edited!";
            }

            return response;
        }

        public async Task<Setting> Load(int id, BaseResponseModel response)
        {
            Setting setting = await Database.Setting.FirstOrDefaultAsync(a => a.Id == id);
            if (setting == null)
            {
                response.Success = false;
                response.Message = "Unknown setting!";
            }

            return setting;
        }

        public SettingModel ConvertToModel(Setting dbModel)
        {
            return new SettingModel
            {
                Id = dbModel.Id,
                SettingName = dbModel.SettingName,
                SettingScope = dbModel.SettingScope,
                Value = dbModel.Value
            };
        }
    }
}