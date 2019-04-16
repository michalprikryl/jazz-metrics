using Database;
using Database.DAO;
using Library.Services.Jazz;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Library.Services.Snapshot
{
    /// <summary>
    /// servis na stahovani dat z Jazz
    /// </summary>
    public class SnapshotService : ISnapshotService
    {
        /// <summary>
        /// defaultni cas pro dalsi beh servisu - 24 hodin
        /// </summary>
        private const int DEFAULT_PERIOD = 1440;

        /// <summary>
        /// databazovy kontext
        /// </summary>
        private readonly JazzMetricsContext _db;
        /// <summary>
        /// servis pro praci s Jazz
        /// </summary>
        private readonly IJazzService _jazzService;

        public SnapshotService(JazzMetricsContext db, IJazzService jazzService)
        {
            _db = db;
            _jazzService = jazzService;
        }

        /// <summary>
        /// stahne a zpracuje data z Jazz
        /// </summary>
        /// <returns></returns>
        public async Task<double> CheckPeriodSetting()
        {
            Setting setting = await _db.Setting.FirstOrDefaultAsync(s => s.SettingScope == "Job" && s.SettingName == "MetricUpdateMinutes");
            if(setting != null)
            {
                return double.TryParse(setting.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double period) ? period : DEFAULT_PERIOD;
            }
            else
            {
                return DEFAULT_PERIOD;
            }
        }

        /// <summary>
        /// zkontroluje, za jak dlouho se ma sluzba znovu spustit
        /// </summary>
        /// <returns></returns>
        public async Task CreateSnapshots()
        {
            Console.WriteLine("{0} -> Start of metrics update.", DateTime.Now.GetDateTimeString());

            int countOfProjects = 0, countOfProjectMetrics = 0;
            foreach (var project in await _db.Project.ToListAsync())
            {
                foreach (var projectMetric in project.ProjectMetric)
                {
                    await _jazzService.CreateSnapshot(projectMetric);

                    await _db.SaveChangesAsync();

                    countOfProjectMetrics++;
                }

                countOfProjects++;
            }

            Console.WriteLine("{0} -> End of metrics update. {1} projects was updated, this was {2} project metrics!", DateTime.Now.GetDateTimeString(), countOfProjects, countOfProjectMetrics);
        }
    }
}
