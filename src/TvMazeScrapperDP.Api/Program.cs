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
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog(ConfigureSerilog);
                });
        }

        private static void ConfigureSerilog(WebHostBuilderContext ctx, LoggerConfiguration loggerConfiguration) =>
            loggerConfiguration
                .WriteTo.Console();
    }
}