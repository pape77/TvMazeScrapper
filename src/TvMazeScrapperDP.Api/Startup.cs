using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using Polly;
using Polly.Extensions.Http;
using Rtl.Configuration.Validation;
using TvMaze.Client;
using TvMaze.Client.Models;
using TvMaze.Client.Options;
using TvMazeScrapperDp.Core.Extensions;
using TvMazeScrapperDp.Core.Models;
using TvMazeScrapperDp.Core.Options;
using TvMazeScrapperDp.Core.ScheduledServices;
using TvMazeScrapperDp.Core.Services;
using TvMazeScrapperDp.Core.Services.Contracts;
using TvMazeScrapperDP.Persistance.MongoDb;
using TvMazeScrapperDP.Persistance.MongoDb.Services;
using TvMazeScrapperDP.Persistance.MongoDb.Services.Contracts;

namespace TvMazeScrapperDP.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var tvMazeClientConfig = _configuration.GetConfig<TvMazeClientOptions>("TvMazeClient");

            services.AddTvMazeClient(tvMazeClientConfig);

            services.AddMappings();
            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddPersistence(_configuration);
            services.AddCore(opts => _configuration.Bind("Scheduling", opts));

            services.AddHealthChecks()
                .AddUrlGroup(tvMazeClientConfig.BaseUri, "TvMazeSource")
                .AddMongoDb(_configuration["MongoDb:ConnectionString"], _configuration["MongoDb:Database"], "MongoDb");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks((PathString)"/health",
                new HealthCheckOptions {ResponseWriter = ResponseWriters.ResponseWriters.WriteFullJsonReportAsync});

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(builder => builder.MapControllers());
        }
    }
}