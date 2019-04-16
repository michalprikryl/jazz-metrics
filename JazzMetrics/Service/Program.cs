using Database;
using Library.Services.Jazz;
using Library.Services.Snapshot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Service
{
    class Program
    {
        public const string ConnectionStringName = "JazzMetricsDatabase";

        static void Main(string[] args)
        {
            IHost host = new HostBuilder()
                 .ConfigureHostConfiguration(configHost =>
                 {
                     configHost.SetBasePath(Directory.GetCurrentDirectory());
                     configHost.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                     configHost.AddCommandLine(args);
                 })
                 .ConfigureAppConfiguration((hostContext, configApp) => //konfigurace appsettings.json
                 {
                     configApp.SetBasePath(Directory.GetCurrentDirectory());
                     configApp.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                     configApp.AddJsonFile($"appsettings.json", true);
                     configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);
                     configApp.AddCommandLine(args);
                 })
                .ConfigureServices((hostContext, services) => //pridani servisu - dependency injection
                {
                    services.AddHostedService<PeriodService>();
                    services.AddScoped<IJazzService, JazzService>();
                    services.AddScoped<ISnapshotService, SnapshotService>();
                    services.AddDbContext<JazzMetricsContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString(ConnectionStringName)));
                })
                .Build();

            host.Run();
        }
    }
}
