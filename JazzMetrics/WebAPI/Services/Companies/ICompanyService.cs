using Database.DAO;
using Library.Models.Company;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Companies
{
    public interface ICompanyService : ICrudOperations<CompanyModel, Company>
    {
    }
}
