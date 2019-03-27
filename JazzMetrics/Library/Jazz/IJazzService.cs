using Database.DAO;
using System.Threading.Tasks;

namespace Library.Jazz
{
    public interface IJazzService
    {
        Task CreateSnapshot(ProjectMetric projectMetric);
    }
}
