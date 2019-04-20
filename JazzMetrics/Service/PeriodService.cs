using Library.Services.Snapshot;
using System;
using System.Diagnostics;
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
            DateTime now = DateTime.Now;
            DateTime fiveOclock = new DateTime(now.Year, now.Month, now.Day + 1, 7, 0, 0);

            TimeSpan timeSpan = fiveOclock - now;
            Console.WriteLine("Next run is scheduled in {0} hours and {1} minutes. (tomorrow at 7:00:00)", timeSpan.Hours, timeSpan.Minutes);

            await Task.Delay(timeSpan);

            while (!stoppingToken.IsCancellationRequested)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                await _snapshotService.CreateSnapshots();

                double minutes = await _snapshotService.CheckPeriodSetting();

                Console.WriteLine("Next run is scheduled in {0} minutes.", minutes);

                stopwatch.Stop();

                await Task.Delay(ToMilliSeconds(minutes, stopwatch.ElapsedMilliseconds), stoppingToken);
            }
        }

        /// <summary>
        /// prevede minuty na ms -> x * 60 * 1000
        /// </summary>
        /// <param name="minutes">cislo v minutach</param>
        /// <returns></returns>
        private int ToMilliSeconds(double minutes, long elapsed)
        {
            return Convert.ToInt32(minutes * 60 * 1000) - unchecked((int)elapsed);
        }
    }
}
