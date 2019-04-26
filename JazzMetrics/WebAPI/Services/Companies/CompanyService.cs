using Database;
using Database.DAO;
using Library;
using Library.Models;
using Library.Models.Company;
using Library.Models.Users;
using Library.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;
using WebAPI.Services.Users;

namespace WebAPI.Services.Companies
{
    /// <summary>
    /// servis pro praci s DB tabulkou Company
    /// </summary>
    public class CompanyService : BaseDatabase, ICompanyService
    {
        /// <summary>
        /// servis pro praci s User
        /// </summary>
        private readonly IUserService _userService;

        public CompanyService(JazzMetricsContext db, IUserService userService) : base(db) => _userService = userService;

        public async Task<BaseResponseModelGetAll<CompanyModel>> GetAll(bool lazy)
        {
            var response = new BaseResponseModelGetAll<CompanyModel> { Values = new List<CompanyModel>() };

            foreach (var item in await Database.Company.ToListAsyncSpecial(c => c.User))
            {
                CompanyModel company = ConvertToModel(item);

                if (!lazy)
                {
                    company.Users = GetUsers(item.User);
                }

                response.Values.Add(company);
            }

            return response;
        }

        public async Task<BaseResponseModelGet<CompanyModel>> Get(int id, bool lazy)
        {
            var response = new BaseResponseModelGet<CompanyModel>();

            Company company = await Load(id, response, false, lazy);
            if (company != null)
            {
                response.Value = ConvertToModel(company);

                if (!lazy)
                {
                    response.Value.Users = GetUsers(company.User);
                }
            }

            return response;
        }

        public async Task<BaseResponseModelPost> Create(CompanyModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (request.Validate())
            {
                Company company = new Company
                {
                    Name = request.Name
                };

                await Database.Company.AddAsync(company);

                await Database.SaveChangesAsync();

                response.Id = company.Id;
                response.Message = "Company was successfully created!";
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Edit(CompanyModel request)
        {
            BaseResponseModel response = new BaseResponseModel();

            if (request.Validate())
            {
                Company company = await Load(request.Id, response);
                if (company != null)
                {
                    company.Name = request.Name;

                    await Database.SaveChangesAsync();

                    response.Message = "Company was successfully edited!";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Drop(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            Company company = await Load(id, response, true, false);
            if (company != null)
            {
                if (company.User.Count == 0)
                {
                    Database.Company.Remove(company);

                    await Database.SaveChangesAsync();

                    response.Message = "Company was successfully deleted!";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Cannot delete company with active users!";
                }
            }

            return response;
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new NotImplementedException();
        }

        public async Task<Company> Load(int id, BaseResponseModel response, bool tracking = true, bool lazy = true)
        {
            Company company = await Database.Company.FirstOrDefaultAsyncSpecial(a => a.Id == id, tracking, !lazy ? (Expression<Func<Company, ICollection<User>>>)(c => c.User) : null);
            if (company == null)
            {
                response.Success = false;
                response.Message = "Unknown company!";
            }

            return company;
        }

        private List<UserModel> GetUsers(ICollection<User> users) => users.Select(u => _userService.ConvertToModel(u)).ToList();

        public CompanyModel ConvertToModel(Company dbModel)
        {
            return new CompanyModel
            {
                Id = dbModel.Id,
                Name = dbModel.Name
            };
        }
    }
}
