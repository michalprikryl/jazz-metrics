using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// abstraktni trida - jako zaklad pro servisy
    /// </summary>
    public abstract class Service : IHostedService, IDisposable
    {
        /// <summary>
        /// task, ktery momentalne probiha
        /// </summary>
        private Task _executingTask;
        /// <summary>
        /// cancelation token - udava, zda se ma task ukoncit
        /// </summary>
        private readonly CancellationTokenSource _stoppingCts;

        public Service()
        {
            _stoppingCts = new CancellationTokenSource();
        }

        /// <summary>
        /// metoda, ktera zajistuje beh jobu servisu
        /// </summary>
        /// <param name="stoppingToken">token o konci - servis byl ukoncen</param>
        /// <returns></returns>
        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        /// <summary>
        /// metoda, ktera se provede pri spusteni servisu - zapocne praci
        /// </summary>
        /// <param name="cancellationToken">token o konci</param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Service was started! Hello World!");

            _executingTask = ExecuteAsync(_stoppingCts.Token);

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// metoda, ktera se provede pri ukonceni servisu - ukonci praci
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }

            Console.WriteLine("Service was stopped! Bye World!");
        }

        /// <summary>
        /// ukonci praci
        /// </summary>
        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}
