using Database.DAO;
using Library.Models.Company;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Companies
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou Company
    /// </summary>
    public interface ICompanyService : ICrudOperations<CompanyModel, Company> { }
}
