using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;


namespace TvMazeScrapperDP.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
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