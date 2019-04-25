using Library.Models.AffectedFields;
using Library.Models.AspiceProcesses;
using Library.Models.AspiceVersions;
using Library.Models.Company;
using Library.Models.Metric;
using Library.Models.MetricType;
using Library.Services.Jazz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Setting.AffectedField;
using WebApp.Models.Setting.AppError;
using WebApp.Models.Setting.Setting;
using WebApp.Models.Setting.AspiceProcess;
using WebApp.Models.Setting.AspiceVersion;
using WebApp.Models.Setting.Company;
using WebApp.Models.Setting.Metric;
using WebApp.Models.Setting.MetricType;
using WebApp.Services.Crud;
using WebApp.Services.Error;
using WebApp.Services.Setting;
using WebApp.Services.Users;
using Library.Models.Token;

namespace WebApp.Controllers
{
    [Route("Setting")]
    public class SettingController : AppController
    {
        private readonly ICrudService _crudService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;

        public SettingController(IErrorService errorService, ICrudService crudService, ISettingService settingService, IUserService userService) : base(errorService)
        {
            _crudService = crudService;
            _userService = userService;
            _settingService = settingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Affected field
        [HttpGet("AffectedField")]
        [Authorize(Roles = RoleSuperAdmin)]
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
        [Authorize(Roles = RoleSuperAdmin)]
        public IActionResult AffectedFieldAdd()
        {
            AffectedFieldWorkModel model = new AffectedFieldWorkModel();

            return View("AffectedField/Add", model);
        }

        [HttpPost("AffectedField/Add")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AffectedFieldAddPost(AffectedFieldWorkModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _crudService.Create(model, Token, SettingService.AffectedFieldEntity);

                AddMessageToModel(model, result.Message, !result.Success);

                if (result.Success)
                {
                    return RedirectToActionWithId(model, "AffectedFieldEdit", "Setting", result.Id);
                }
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View("AffectedField/Add", model);
        }

        [HttpGet("AffectedField/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AffectedFieldEdit(int id)
        {
            AffectedFieldWorkModel model = new AffectedFieldWorkModel();

            CheckTempData(model);

            var result = await _crudService.Get<AffectedFieldModel>(id, Token, SettingService.AffectedFieldEntity);
            if (result.Success)
            {
                model.Id = id;
                model.Name = result.Value.Name;
                model.Description = result.Value.Description;
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("AffectedField/Edit", model);
        }

        [HttpPost("AffectedField/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
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
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AffectedFieldDelete(int id)
        {
            return Json(await _crudService.Drop(id, Token, SettingService.AffectedFieldEntity));
        }
        #endregion

        #region Metric types
        [HttpGet("MetricType")]
        [Authorize(Roles = RoleSuperAdmin)]
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
        [Authorize(Roles = RoleSuperAdmin)]
        public IActionResult MetricTypeAdd()
        {
            MetricTypeWorkModel model = new MetricTypeWorkModel();

            return View("MetricType/Add", model);
        }

        [HttpPost("MetricType/Add")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> MetricTypeAddPost(MetricTypeWorkModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _crudService.Create(model, Token, SettingService.MetricTypeEntity);

                AddMessageToModel(model, result.Message, !result.Success);

                if (result.Success)
                {
                    return RedirectToActionWithId(model, "MetricTypeEdit", "Setting", result.Id);
                }
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View("MetricType/Add", model);
        }

        [HttpGet("MetricType/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> MetricTypeEdit(int id)
        {
            MetricTypeWorkModel model = new MetricTypeWorkModel();

            CheckTempData(model);

            var result = await _crudService.Get<MetricTypeModel>(id, Token, SettingService.MetricTypeEntity);
            if (result.Success)
            {
                model.Id = id;
                model.Name = result.Value.Name;
                model.Description = result.Value.Description;
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("MetricType/Edit", model);
        }

        [HttpPost("MetricType/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
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
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> MetricTypeDelete(int id)
        {
            return Json(await _crudService.Drop(id, Token, SettingService.MetricTypeEntity));
        }
        #endregion

        #region Automotive SPICE versions
        [HttpGet("AspiceVersion")]
        [Authorize(Roles = RoleSuperAdmin)]
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
        [Authorize(Roles = RoleSuperAdmin)]
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
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AspiceVersionAddPost(AspiceVersionWorkModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _crudService.Create(model, Token, SettingService.AspiceVersionEntity);

                AddMessageToModel(model, result.Message, !result.Success);

                if (result.Success)
                {
                    return RedirectToActionWithId(model, "AspiceVersionEdit", "Setting", result.Id);
                }
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View("AspiceVersion/Add", model);
        }

        [HttpGet("AspiceVersion/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AspiceVersionEdit(int id)
        {
            AspiceVersionWorkModel model = new AspiceVersionWorkModel();

            CheckTempData(model);

            var result = await _crudService.Get<AspiceVersionModel>(id, Token, SettingService.AspiceVersionEntity);
            if (result.Success)
            {
                model.Id = id;
                model.Description = result.Value.Description;
                model.VersionNumber = result.Value.VersionNumber;
                model.ReleaseDate = result.Value.ReleaseDate.ToShortDateString();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("AspiceVersion/Edit", model);
        }

        [HttpPost("AspiceVersion/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
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
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AspiceVersionTypeDelete(int id)
        {
            return Json(await _crudService.Drop(id, Token, SettingService.AspiceVersionEntity));
        }
        #endregion

        #region Automotive SPICE processes
        [HttpGet("AspiceProcess")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AspiceProcess()
        {
            AspiceProcessListModel model = new AspiceProcessListModel();

            var result = await _crudService.GetAll<AspiceProcessModel>(Token, SettingService.AspiceProcessEntity, false);
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
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AspiceProcessAdd()
        {
            AspiceProcessWorkModel model = new AspiceProcessWorkModel();

            await GetAspiceVersions(model);

            return View("AspiceProcess/Add", model);
        }

        [HttpPost("AspiceProcess/Add")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AspiceProcessAddPost(AspiceProcessWorkModel model)
        {
            Task select = GetAspiceVersions(model);

            if (ModelState.IsValid)
            {
                var result = await _crudService.Create(model, Token, SettingService.AspiceProcessEntity);

                AddMessageToModel(model, result.Message, !result.Success);

                if (result.Success)
                {
                    return RedirectToActionWithId(model, "AspiceProcessEdit", "Setting", result.Id);
                }
            }
            else
            {
                AddModelStateErrors(model);
            }

            Task.WaitAll(select);

            return View("AspiceProcess/Add", model);
        }

        [HttpGet("AspiceProcess/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AspiceProcessEdit(int id)
        {
            AspiceProcessWorkModel model = new AspiceProcessWorkModel();

            CheckTempData(model);

            Task select = GetAspiceVersions(model);

            var result = await _crudService.Get<AspiceProcessModel>(id, Token, SettingService.AspiceProcessEntity);
            if (result.Success)
            {
                model.Id = id;
                model.Name = result.Value.Name;
                model.Shortcut = result.Value.Shortcut;
                model.Description = result.Value.Description;
                model.AspiceVersionId = result.Value.AspiceVersionId.ToString();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            Task.WaitAll(select);

            return View("AspiceProcess/Edit", model);
        }

        [HttpPost("AspiceProcess/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
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
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AspiceProcessDelete(int id)
        {
            return Json(await _crudService.Drop(id, Token, SettingService.AspiceProcessEntity));
        }

        private async Task GetAspiceVersions(AspiceProcessWorkModel model)
        {
            model.AspiceVersions = await _settingService.GetAspiceVersionsForSelect(Token);

            if (model.AspiceVersions == null || model.AspiceVersions.Count == 0)
            {
                AddMessageToModel(model, "Cannot retrieve Automotive SPICE versions, press F5 please.");
            }
        }
        #endregion

        #region Metrics
        [HttpGet("Metric")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> Metric()
        {
            MetricListModel model = new MetricListModel();

            var result = await _crudService.GetAll<MetricModel>(Token, SettingService.MetricEntity, false);
            if (result.Success)
            {
                model.Metrics = result.Values.Select(a =>
                    new MetricViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Public = a.Public,
                        Description = a.Description,
                        Identificator = a.Identificator,
                        AffectedFieldId = a.AffectedField.Id,
                        AffectedField = a.AffectedField.ToString(),
                        AspiceProcessId = a.AspiceProcess.Id,
                        AspiceProcess = a.AspiceProcess.ToString(),
                        MetricTypeId = a.MetricType.Id,
                        MetricType = a.MetricType.ToString(),
                        CompanyId = a.CompanyId
                    }).ToList();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("Metric/Index", model);
        }

        [HttpGet("Metric/Detail/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> MetricDetail(int id)
        {
            MetricDetailViewModel model = new MetricDetailViewModel();

            var result = await _crudService.Get<MetricModel>(id, Token, SettingService.MetricEntity, false);
            if (result.Success)
            {
                model.Id = id;
                model.Name = result.Value.Name;
                model.Public = result.Value.Public;
                model.CompanyId = result.Value.CompanyId;
                model.Description = result.Value.Description;
                model.Identificator = result.Value.Identificator;
                model.RequirementGroup = result.Value.RequirementGroup;
                model.MetricType = result.Value.MetricType.ToString();
                model.AspiceProcess = result.Value.AspiceProcess.ToString();
                model.AffectedField = result.Value.AffectedField.ToString();

                model.LoadMetricColumns(result.Value.Columns);
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("Metric/Detail", model);
        }

        [HttpGet("Metric/Add")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> MetricAdd()
        {
            MetricWorkModel model = new MetricWorkModel();

            await GetMetricSelects(model);

            return View("Metric/Add", model);
        }

        [HttpPost("Metric/Add")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> MetricAddPost(MetricWorkModel model)
        {
            Task select = GetMetricSelects(model);

            model.DropDeletedColumns();

            if (ModelState.IsValid)
            {
                var result = await _crudService.Create(model.TranslateToMetricModel(), Token, SettingService.MetricEntity);

                AddMessageToModel(model, result.Message, !result.Success);

                if (result.Success)
                {
                    return RedirectToActionWithId(model, "MetricEdit", "Setting", result.Id);
                }
            }
            else
            {
                AddModelStateErrors(model);
            }

            Task.WaitAll(select);

            return View("Metric/Add", model);
        }

        [HttpGet("Metric/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> MetricEdit(int id)
        {
            MetricWorkModel model = new MetricWorkModel();

            CheckTempData(model);

            Task select = GetMetricSelects(model);

            var result = await _crudService.Get<MetricModel>(id, Token, SettingService.MetricEntity, false); //lazy kvuli columns
            if (result.Success)
            {
                if (result.Value.CompanyId == MyUser.CompanyId)
                {
                    model.Id = id;
                    model.Name = result.Value.Name;
                    model.Public = result.Value.Public;
                    model.Description = result.Value.Description;
                    model.RequirementGroup = result.Value.RequirementGroup;
                    model.Identificator = result.Value.Identificator;
                    model.MetricTypeId = result.Value.MetricTypeId.ToString();
                    model.AspiceProcessId = result.Value.AspiceProcessId.ToString();
                    model.AffectedFieldId = result.Value.AffectedFieldId.ToString();

                    model.LoadMetricColumns(result.Value.Columns);
                }
                else
                {
                    model.CanView = false;
                    AddMessageToModel(model, "You can't edit this metric!");
                }
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            Task.WaitAll(select);

            return View("Metric/Edit", model);
        }

        [HttpPost("Metric/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> MetricEditPost(int id, MetricWorkModel model)
        {
            Task select = GetMetricSelects(model);

            model.DropDeletedColumns();

            if (ModelState.IsValid)
            {
                var result = await _crudService.Edit(id, model.TranslateToMetricModel(), Token, SettingService.MetricEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            Task.WaitAll(select);

            return View("Metric/Edit", model);
        }

        [HttpPost("Metric/Delete/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> MetricDelete(int id)
        {
            return Json(await _crudService.Drop(id, Token, SettingService.MetricEntity));
        }

        [HttpPost("Metric/AddColumn")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public IActionResult AddNewMetricColumn([FromBody] NewMetricColumn model)
        {
            if (model.Type.ToLower().Contains("coverage"))
            {
                return PartialView("Metric/Partials/CoverageMetricColumn",
                    new MetricCoverageColumn
                    {
                        Index = model.Index,
                        Value = JazzService.ANY_VALUE,
                        DivisorValue = JazzService.ALL_VALUES,
                        FieldName = JazzService.FIELD_VALUE,
                        DivisorFieldName = string.Empty,
                        CoverageName = "Coverage"
                    });
            }
            else
            {
                return PartialView("Metric/Partials/MetricColumn",
                    new MetricColumn
                    {
                        Index = model.Index,
                        Value = JazzService.ANY_VALUE,
                        FieldName = JazzService.FIELD_VALUE,
                        NumberFieldName = JazzService.FIELD_COUNT
                    });
            }
        }

        private async Task GetMetricSelects(MetricWorkModel model)
        {
            Task[] tasks = new Task[3];

            tasks[0] = _settingService.GetAspiceProcessesForSelect(Token).ContinueWith(t =>
            {
                model.AspiceProcesses = t.Result;

                if (model.AspiceProcesses == null || model.AspiceProcesses.Count == 0)
                {
                    AddMessageToModel(model, "Cannot retrieve Automotive SPICE processes, press F5 please.");
                }
            });

            tasks[1] = _settingService.GetMetricTypesForSelect(Token).ContinueWith(t =>
            {
                model.MetricTypes = t.Result;

                if (model.MetricTypes == null || model.MetricTypes.Count == 0)
                {
                    AddMessageToModel(model, "Cannot retrieve metric types, press F5 please.");
                }
            });

            tasks[2] = _settingService.GetAffectedFieldsForSelect(Token).ContinueWith(t =>
            {
                model.AffectedFields = t.Result;

                if (model.AffectedFields == null || model.AffectedFields.Count == 0)
                {
                    AddMessageToModel(model, "Cannot retrieve metric afected fields, press F5 please.");
                }
            });

            await Task.WhenAll(tasks);
        }
        #endregion

        #region Company
        [HttpGet("Company")]
        public async Task<IActionResult> Company()
        {
            CompanyViewModel model = new CompanyViewModel();

            CheckTempData(model);

            if (MyUser.CompanyId.HasValue)
            {
                var result = await _crudService.Get<CompanyModel>(MyUser.CompanyId.Value, Token, SettingService.CompanyEntity, false);
                if (result.Success)
                {
                    model.Id = result.Value.Id;
                    model.Name = result.Value.Name;
                    model.Users = result.Value.Users.Select(a =>
                        new CompanyUser
                        {
                            UserId = a.Id,
                            Username = a.Username,
                            UserInfo = a.ToString(),
                            Admin = a.Admin
                        }).ToList();
                }
                else
                {
                    AddMessageToModel(model, result.Message);
                }
            }
            else
            {
                model.CanView = false;
            }

            return View("Company/Index", model);
        }

        [HttpPost("Company/Add")]
        public async Task<IActionResult> CompanyAdd(CompanyModel model)
        {
            ViewModel viewModel = new ViewModel();

            var result = await _crudService.Create(model, Token, SettingService.CompanyEntity);
            if (result.Success)
            {
                var userResult = await _crudService.PartialEdit(MyUser.UserId, CreatePatchList(CreatePatchModel("companyId", result.Id.ToString()), CreatePatchModel("userRoleId", RoleAdmin)),
                    Token, UserService.UserEntity);

                var user = MyUser;
                user.CompanyId = result.Id;
                user.Role = RoleAdmin;

                TokenModel refresh = await _userService.RefreshToken(new TokenRequestModel { UserId = user.UserId, UserRole = RoleAdmin }, Token);

                await UserLogin(user, refresh.Token);

                AddMessageToModel(viewModel, result.Message, !result.Success);
            }
            else
            {
                AddMessageToModel(viewModel, result.Message);
            }

            AddViewModelToTempData(viewModel);

            return RedirectToAction("Company");
        }

        [HttpPost("CompanyUser/Add")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> CompanyUserAdd(CompanyUserModel model)
        {
            ViewModel viewModel = new ViewModel();

            var userResult = await _userService.FindUserIdByUsername(model.Username, Token);
            if (userResult.Success)
            {
                var result = await _crudService.PartialEdit(userResult.Id, CreatePatchList(CreatePatchModel("companyId", model.CompanyId.ToString())), Token, UserService.UserEntity);
                if (result.Success)
                {
                    AddMessageToModel(viewModel, "User was successfully added to company.", false);
                }
                else
                {
                    AddMessageToModel(viewModel, result.Message);
                }
            }
            else
            {
                AddMessageToModel(viewModel, userResult.Message);
            }

            AddViewModelToTempData(viewModel);

            return RedirectToAction("Company");
        }

        [HttpPost("CompanyUser/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> CompanyUserUpdate(int id)
        {
            return Json(await _crudService.PartialEdit(id, CreatePatchList(CreatePatchModel("userRoleId", "")), Token, UserService.UserEntity));
        }

        [HttpPost("CompanyUser/Delete/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> CompanyUserDelete(int id)
        {
            return Json(await _crudService.PartialEdit(id, CreatePatchList(CreatePatchModel("companyId", null)), Token, UserService.UserEntity));
        }
        #endregion

        #region App error
        [HttpGet("AppError")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AppError()
        {
            AppErrorListModel model = new AppErrorListModel();

            var result = await _crudService.GetAll<AppErrorViewModel>(Token, SettingService.AppErrorEntity);
            if (result.Success)
            {
                model.AppErrors = result.Values.Where(e => !e.Deleted).Select(e =>
                    new AppErrorViewModel
                    {
                        Id = e.Id,
                        AppInfo = e.AppInfo,
                        Exception = e.Exception,
                        Function = e.Function,
                        InnerException = e.InnerException,
                        Message = e.Message,
                        Module = e.Module,
                        Solved = e.Solved,
                        Time = e.Time
                    }).ToList();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("AppError/Index", model);
        }

        [HttpPost("AppError/Solve")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AppErrorSolve([FromBody]AppErrorUpdateModel model)
        {
            return Json(await _crudService.PartialEdit(model.Id, CreatePatchList(CreatePatchModel("solved", model.Solve.ToString())), Token, SettingService.AppErrorEntity));
        }

        [HttpPost("AppError/Delete/{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> AppErrorDelete(int id)
        {
            return Json(await _crudService.Drop(id, Token, SettingService.AppErrorEntity));
        }

        [HttpPost("AppError/Info")]
        [Authorize(Roles = RoleSuperAdmin)]
        public IActionResult AppErrorInfo([FromBody]AppErrorViewModel model)
        {
            return PartialView("AppError/Partials/AppErrorInfo", model);
        }
        #endregion

        #region Setting
        [HttpGet("Setting")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> Setting()
        {
            SettingListModel model = new SettingListModel();

            var result = await _crudService.GetAll<SettingViewModel>(Token, SettingService.SettingEntity);
            if (result.Success)
            {
                model.Settings = result.Values.Select(s =>
                    new SettingViewModel
                    {
                        Id = s.Id,
                        SettingName = s.SettingName,
                        SettingScope = s.SettingScope,
                        Value = s.Value
                    }).OrderBy(s => s.SettingScope).ThenBy(s => s.SettingName).ToList();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("Setting/Index", model);
        }

        [HttpPost("Setting/ChangeValue")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<IActionResult> SettingChangeValue([FromBody]SettingUpdateModel model)
        {
            return Json(await _crudService.PartialEdit(model.Id, CreatePatchList(CreatePatchModel("value", model.Value)), Token, SettingService.SettingEntity));
        }
        #endregion
    }
}