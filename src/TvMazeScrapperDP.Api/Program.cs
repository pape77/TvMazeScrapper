using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;


namespace TvMazeScrapperDP.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var webHost = CreateHostBuilder(args).Build();
            await webHost.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                .UseSerilog(ConfigureSerilog);
        }

        private static void ConfigureSerilog(HostBuilderContext ctx, LoggerConfiguration loggerConfiguration) =>
            loggerConfiguration
                .WriteTo.Console();
    }
}