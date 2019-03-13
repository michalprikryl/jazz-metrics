using Database.DAO;
using WebAPI.Models.Projects;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Projects
{
    public interface IProjectService : ICrudOperations<ProjectModel, Project>
    {
        int CurrentUserId { get; set; }
    }
}
