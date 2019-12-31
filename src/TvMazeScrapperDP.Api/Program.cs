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

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog(ConfigureSerilog);
        }

        private static void ConfigureSerilog(WebHostBuilderContext ctx, LoggerConfiguration loggerConfiguration) =>
            loggerConfiguration
                .WriteTo.Console();
    }
}