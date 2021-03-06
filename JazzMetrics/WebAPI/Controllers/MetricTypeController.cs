﻿using Library.Models;
using Library.Models.MetricType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.MetricTypes;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricTypeController : MainController
    {
        private readonly IMetricTypeService _metricTypeService;

        public MetricTypeController(IHelperService helperService, IMetricTypeService metricTypeService) : base(helperService) => _metricTypeService = metricTypeService;

        [HttpGet("{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<ActionResult<BaseResponseModelGet<MetricTypeModel>>> Get(int id, bool lazy = true)
        {
            return await _metricTypeService.Get(id, lazy);
        }

        [HttpGet]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<ActionResult<BaseResponseModelGetAll<MetricTypeModel>>> Get(bool lazy = true)
        {
            return await _metricTypeService.GetAll(lazy);
        }

        [HttpPost]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<ActionResult<BaseResponseModelPost>> Post([FromBody]MetricTypeModel model)
        {
            return await _metricTypeService.Create(model);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, [FromBody]MetricTypeModel model)
        {
            return await _metricTypeService.Edit(model);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _metricTypeService.Drop(id);
        }
    }
}