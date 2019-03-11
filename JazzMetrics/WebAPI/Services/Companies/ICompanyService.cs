using Database.DAO;
using WebAPI.Models.Company;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Companies
{
    public interface ICompanyService : ICrudOperations<CompanyModel, Company>
    {
    }
}
