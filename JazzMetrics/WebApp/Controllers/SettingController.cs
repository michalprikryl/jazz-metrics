using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Setting.AffectedField;
using WebApp.Models.Setting.AspiceVersion;
using WebApp.Models.Setting.MetricType;
using WebApp.Services.Error;
using WebApp.Services.Setting;

namespace WebApp.Controllers
{
    [Route("Setting")]
    [Authorize(Roles = "super-admin")]
    public class SettingController : AppController
    {
        private readonly ISettingService _settingService;

        public SettingController(IErrorService errorService, ISettingService settingService) : base(errorService) => _settingService = settingService;

        public IActionResult Index()
        {
            return View();
        }

        #region Affected field
        [HttpGet("AffectedField")]
        public async Task<IActionResult> AffectedField()
        {
            AffectedFieldListModel model = new AffectedFieldListModel();

            var result = await _settingService.GetAllAffectedFields(Token);
            if (result.Success)
            {
                model.AffectedFields = result.Values.Select(v => 
                    new AffectedFieldViewModel
                    {
                        Id = v.Id,
                        Name = v.Name,
                        Description = v.Description
                    }).ToList();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("AffectedField/Index", model);
        }

        [HttpGet("AffectedField/Add")]
        public IActionResult AffectedFieldAdd()
        {
            AffectedFieldWorkModel model = new AffectedFieldWorkModel();

            return View("AffectedField/Add", model);
        }

        [HttpPost("AffectedField/Add")]
        public async Task<IActionResult> AffectedFieldAddPost(AffectedFieldWorkModel model)
        {
            var result = await _settingService.CreateAffectedField(model, Token);

            AddMessageToModel(model, result.Message, !result.Success);

            return View("AffectedField/Add", model);
        }

        [HttpGet("AffectedField/Edit/{id}")]
        public async Task<IActionResult> AffectedFieldEdit(int id)
        {
            AffectedFieldWorkModel model = new AffectedFieldWorkModel();

            AffectedFieldModel result = await _settingService.GetAffectedField(id, Token);
            if (result.Success)
            {
                model.Id = id;
                model.Name = result.Name;
                model.Description = result.Description;
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("AffectedField/Edit", model);
        }

        [HttpPost("AffectedField/Edit/{id}")]
        public async Task<IActionResult> AffectedFieldEditPost(int id, AffectedFieldWorkModel model)
        {
            var result = await _settingService.EditAffectedField(model, Token);

            AddMessageToModel(model, result.Message, !result.Success);

            return View("AffectedField/Edit", model);
        }

        [HttpPost("AffectedField/Delete/{id}")]
        public async Task<IActionResult> AffectedFieldDelete(int id)
        {
            return Json(await _settingService.DropAffectedField(id, Token));
        }
        #endregion

        #region Metric types
        [HttpGet("MetricType")]
        public async Task<IActionResult> MetricType()
        {
            MetricTypeListModel model = new MetricTypeListModel();

            var result = await _settingService.GetAllMetricTypes(Token);
            if (result.Success)
            {
                model.MetricTypes = result.Values.Select(v =>
                    new MetricTypeViewModel
                    {
                        Id = v.Id,
                        Name = v.Name,
                        Description = v.Description
                    }).ToList();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("MetricType/Index", model);
        }

        [HttpGet("MetricType/Add")]
        public IActionResult MetricTypeAdd()
        {
            MetricTypeWorkModel model = new MetricTypeWorkModel();

            return View("MetricType/Add", model);
        }

        [HttpPost("MetricType/Add")]
        public async Task<IActionResult> MetricTypeAddPost(MetricTypeWorkModel model)
        {
            var result = await _settingService.CreateMetricType(model, Token);

            AddMessageToModel(model, result.Message, !result.Success);

            return View("MetricType/Add", model);
        }

        [HttpGet("MetricType/Edit/{id}")]
        public async Task<IActionResult> MetricTypeEdit(int id)
        {
            MetricTypeWorkModel model = new MetricTypeWorkModel();

            MetricTypeModel result = await _settingService.GetMetricType(id, Token);
            if (result.Success)
            {
                model.Id = id;
                model.Name = result.Name;
                model.Description = result.Description;
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("MetricType/Edit", model);
        }

        [HttpPost("MetricType/Edit/{id}")]
        public async Task<IActionResult> MetricTypeEditPost(int id, MetricTypeWorkModel model)
        {
            var result = await _settingService.EditMetricType(model, Token);

            AddMessageToModel(model, result.Message, !result.Success);

            return View("MetricType/Edit", model);
        }

        [HttpPost("MetricType/Delete/{id}")]
        public async Task<IActionResult> MetricTypeDelete(int id)
        {
            return Json(await _settingService.DropMetricType(id, Token));
        }
        #endregion

        #region Automotive SPICE versions
        [HttpGet("AspiceVersion")]
        public async Task<IActionResult> AspiceVersion()
        {
            AspiceVersionListModel model = new AspiceVersionListModel();

            var result = await _settingService.GetAllAspiceVersions(Token);
            if (result.Success)
            {
                model.AspiceVersions = result.Values.Select(a =>
                    new AspiceVersionViewModel
                    {
                        Id = a.Id,
                        ReleaseDate = a.ReleaseDate,
                        Description = a.Description,
                        VersionNumber = a.VersionNumber
                    }).ToList();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("AspiceVersion/Index", model);
        }

        [HttpGet("AspiceVersion/Add")]
        public IActionResult AspiceVersionAdd()
        {
            AspiceVersionWorkModel model = new AspiceVersionWorkModel { ReleaseDate = DateTime.Now, VersionNumber = 3 };

            return View("AspiceVersion/Add", model);
        }

        [HttpPost("AspiceVersion/Add")]
        public async Task<IActionResult> AspiceVersionAddPost(AspiceVersionWorkModel model)
        {
            var result = await _settingService.CreateAspiceVersion(model, Token);

            AddMessageToModel(model, result.Message, !result.Success);

            return View("AspiceVersion/Add", model);
        }

        [HttpGet("AspiceVersion/Edit/{id}")]
        public async Task<IActionResult> AspiceVersionEdit(int id)
        {
            AspiceVersionWorkModel model = new AspiceVersionWorkModel();

            AspiceVersionModel result = await _settingService.GetAspiceVersion(id, Token);
            if (result.Success)
            {
                model.Id = id;
                model.ReleaseDate = result.ReleaseDate;
                model.Description = result.Description;
                model.VersionNumber = result.VersionNumber;
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("AspiceVersion/Edit", model);
        }

        [HttpPost("AspiceVersion/Edit/{id}")]
        public async Task<IActionResult> AspiceVersionEditPost(int id, AspiceVersionWorkModel model)
        {
            var result = await _settingService.EditAspiceVersion(model, Token);

            AddMessageToModel(model, result.Message, !result.Success);

            return View("AspiceVersion/Edit", model);
        }

        [HttpPost("AspiceVersion/Delete/{id}")]
        public async Task<IActionResult> AspiceVersionTypeDelete(int id)
        {
            return Json(await _settingService.DropAspiceVersion(id, Token));
        }
        #endregion
    }
}