using Database.DAO;
using System.Threading.Tasks;

namespace Library.Services.Jazz
{
    /// <summary>
    /// interface pro tridu pro stazeni zpracovani dat z Jazzu
    /// </summary>
    public interface IJazzService
    {
        /// <summary>
        /// vytvori snapshot dane projektove metriky
        /// </summary>
        /// <param name="projectMetric">projektova metrika</param>
        /// <returns></returns>
        Task CreateSnapshot(ProjectMetric projectMetric);
    }
}
