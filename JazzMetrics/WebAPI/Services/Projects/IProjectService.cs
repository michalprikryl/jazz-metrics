using Database.DAO;
using Library.Models.Projects;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Projects
{
    public interface IProjectService : ICrudOperations<ProjectModel, Project>, IUser { }
}
