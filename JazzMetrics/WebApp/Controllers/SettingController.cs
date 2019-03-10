using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Setting.AffectedField;
using WebApp.Models.Setting.AspiceProcess;
using WebApp.Models.Setting.AspiceVersion;
using WebApp.Models.Setting.MetricType;
using WebApp.Services.Crud;
using WebApp.Services.Error;
using WebApp.Services.Setting;

namespace WebApp.Controllers
{
    [Route("Setting")]
    [Authorize(Roles = "super-admin")]
    public class SettingController : AppController
    {
        private readonly ICrudService _crudService;
        private readonly ISettingService _settingService;

        public SettingController(IErrorService errorService, ICrudService crudService, ISettingService settingService) : base(errorService)
        {
            _crudService = crudService;
            _settingService = settingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Affected field
        [HttpGet("AffectedField")]
        public async Task<IActionResult> AffectedField()
        {
            AffectedFieldListModel model = new AffectedFieldListModel();

            var result = await _crudService.GetAll<AffectedFieldModel>(Token, SettingService.AffectedFieldEntity);
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
            if (ModelState.IsValid)
            {
                var result = await _crudService.Create(model, Token, SettingService.AffectedFieldEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View("AffectedField/Add", model);
        }

        [HttpGet("AffectedField/Edit/{id}")]
        public async Task<IActionResult> AffectedFieldEdit(int id)
        {
            AffectedFieldWorkModel model = new AffectedFieldWorkModel();

            AffectedFieldModel result = await _crudService.Get<AffectedFieldModel>(id, Token, SettingService.AffectedFieldEntity);
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
            if (ModelState.IsValid)
            {
                var result = await _crudService.Edit(id, model, Token, SettingService.AffectedFieldEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View("AffectedField/Edit", model);
        }

        [HttpPost("AffectedField/Delete/{id}")]
        public async Task<IActionResult> AffectedFieldDelete(int id)
        {
            return Json(await _crudService.Drop(id, Token, SettingService.AffectedFieldEntity));
        }
        #endregion

        #region Metric types
        [HttpGet("MetricType")]
        public async Task<IActionResult> MetricType()
        {
            MetricTypeListModel model = new MetricTypeListModel();

            var result = await _crudService.GetAll<MetricTypeModel>(Token, SettingService.MetricTypeEntity);
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
            if (ModelState.IsValid)
            {
                var result = await _crudService.Create(model, Token, SettingService.MetricTypeEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View("MetricType/Add", model);
        }

        [HttpGet("MetricType/Edit/{id}")]
        public async Task<IActionResult> MetricTypeEdit(int id)
        {
            MetricTypeWorkModel model = new MetricTypeWorkModel();

            MetricTypeModel result = await _crudService.Get<MetricTypeModel>(id, Token, SettingService.MetricTypeEntity);
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
            if (ModelState.IsValid)
            {
                var result = await _crudService.Edit(id, model, Token, SettingService.MetricTypeEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View("MetricType/Edit", model);
        }

        [HttpPost("MetricType/Delete/{id}")]
        public async Task<IActionResult> MetricTypeDelete(int id)
        {
            return Json(await _crudService.Drop(id, Token, SettingService.MetricTypeEntity));
        }
        #endregion

        #region Automotive SPICE versions
        [HttpGet("AspiceVersion")]
        public async Task<IActionResult> AspiceVersion()
        {
            AspiceVersionListModel model = new AspiceVersionListModel();

            var result = await _crudService.GetAll<AspiceVersionModel>(Token, SettingService.AspiceVersionEntity);
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
            AspiceVersionWorkModel model = new AspiceVersionWorkModel
            {
                ReleaseDate = DateTime.Now.ToShortDateString(),
                VersionNumber = 3
            };

            return View("AspiceVersion/Add", model);
        }

        [HttpPost("AspiceVersion/Add")]
        public async Task<IActionResult> AspiceVersionAddPost(AspiceVersionWorkModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _crudService.Create(model, Token, SettingService.AspiceVersionEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View("AspiceVersion/Add", model);
        }

        [HttpGet("AspiceVersion/Edit/{id}")]
        public async Task<IActionResult> AspiceVersionEdit(int id)
        {
            AspiceVersionWorkModel model = new AspiceVersionWorkModel();

            AspiceVersionModel result = await _crudService.Get<AspiceVersionModel>(id, Token, SettingService.AspiceVersionEntity);
            if (result.Success)
            {
                model.Id = id;
                model.ReleaseDate = result.ReleaseDate.ToShortDateString();
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
            if (ModelState.IsValid)
            {
                var result = await _crudService.Edit(id, model, Token, SettingService.AspiceVersionEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View("AspiceVersion/Edit", model);
        }

        [HttpPost("AspiceVersion/Delete/{id}")]
        public async Task<IActionResult> AspiceVersionTypeDelete(int id)
        {
            return Json(await _crudService.Drop(id, Token, SettingService.AspiceVersionEntity));
        }
        #endregion

        #region Automotive SPICE processes
        [HttpGet("AspiceProcess")]
        public async Task<IActionResult> AspiceProcess()
        {
            AspiceProcessListModel model = new AspiceProcessListModel();

            var result = await _crudService.GetAll<AspiceProcessModel>(Token, SettingService.AspiceProcessEntity);
            if (result.Success)
            {
                model.AspiceProcesses = result.Values.Select(a =>
                    new AspiceProcessViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Shortcut = a.Shortcut,
                        Description = a.Description,
                        AspiceVersionId = a.AspiceVersion.Id,
                        AspiceVersion = a.AspiceVersion.ToString(),
                    }).ToList();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("AspiceProcess/Index", model);
        }

        [HttpGet("AspiceProcess/Add")]
        public async Task<IActionResult> AspiceProcessAdd()
        {
            AspiceProcessWorkModel model = new AspiceProcessWorkModel();

            await GetAspiceVersions(model);

            return View("AspiceProcess/Add", model);
        }

        [HttpPost("AspiceProcess/Add")]
        public async Task<IActionResult> AspiceProcessAddPost(AspiceProcessWorkModel model)
        {
            Task select = GetAspiceVersions(model);

            if (ModelState.IsValid)
            {
                var result = await _crudService.Create(model, Token, SettingService.AspiceProcessEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            Task.WaitAll(select);

            return View("AspiceProcess/Add", model);
        }

        [HttpGet("AspiceProcess/Edit/{id}")]
        public async Task<IActionResult> AspiceProcessEdit(int id)
        {
            AspiceProcessWorkModel model = new AspiceProcessWorkModel();

            Task select = GetAspiceVersions(model);

            AspiceProcessModel result = await _crudService.Get<AspiceProcessModel>(id, Token, SettingService.AspiceProcessEntity);
            if (result.Success)
            {
                model.Id = id;
                model.Name = result.Name;
                model.Shortcut = result.Shortcut;
                model.Description = result.Description;
                model.AspiceVersionId = result.AspiceVersion.Id.ToString();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            Task.WaitAll(select);

            return View("AspiceProcess/Edit", model);
        }

        [HttpPost("AspiceProcess/Edit/{id}")]
        public async Task<IActionResult> AspiceProcessEditPost(int id, AspiceProcessWorkModel model)
        {
            Task select = GetAspiceVersions(model);

            if (ModelState.IsValid)
            {
                var result = await _crudService.Edit(id, model, Token, SettingService.AspiceProcessEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            Task.WaitAll(select);

            return View("AspiceProcess/Edit", model);
        }

        [HttpPost("AspiceProcess/Delete/{id}")]
        public async Task<IActionResult> AspiceProcessDelete(int id)
        {
            return Json(await _crudService.Drop(id, Token, SettingService.AspiceProcessEntity));
        }
        #endregion

        private async Task GetAspiceVersions(AspiceProcessWorkModel model)
        {
            model.AspiceVersions = await _settingService.GetAspiceVersionsForSelect(Token);

            if (model.AspiceVersions == null || model.AspiceVersions.Count == 0)
            {
                AddMessageToModel(model, "Cannot retrieve Automotive SPICE versions, press F5 please.");
            }
        }
    }
}