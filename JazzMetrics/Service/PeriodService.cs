using Library.Services.Snapshot;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// implementace abstraktni tridy servisu - zajistuje kompletni beh
    /// </summary>
    public class PeriodService : Service
    {
        /// <summary>
        /// servis pro praci
        /// </summary>
        private readonly ISnapshotService _snapshotService;

        public PeriodService(ISnapshotService snapshotService)
        {
            _snapshotService = snapshotService;
        }

        /// <summary>
        /// metoda, ktera zajistuje beh jobu servisu
        /// </summary>
        /// <param name="stoppingToken">token o konci - servis byl ukoncen</param>
        /// <returns></returns>
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _snapshotService.CreateSnapshots();

                double minutes = await _snapshotService.CheckPeriodSetting();

                Console.WriteLine("Next run is scheduled in {0} minutes.", minutes);

                await Task.Delay(ToMilliSeconds(minutes), stoppingToken);
            }
        }

        /// <summary>
        /// prevede minuty na ms -> x * 60 * 1000
        /// </summary>
        /// <param name="minutes">cislo v minutach</param>
        /// <returns></returns>
        private int ToMilliSeconds(double minutes)
        {
            return Convert.ToInt32(minutes * 60 * 1000);
        }
    }
}
