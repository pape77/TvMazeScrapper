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
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NodaTime;
using Polly;
using Polly.Extensions.Http;
using Rtl.Configuration.Validation;
using TvMaze.Client;
using TvMaze.Client.Models;
using TvMazeScrapperDp.Core.Models;
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
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Latest);
            var tvMazeClientConfig = _configuration.GetConfig<TvMazeClientConfig>("TvMazeClient");

            services
                .AddTvMazeClient(tvMazeClientConfig)
                .AddPolicyHandler(GetHttpPolicies());

            services.AddMappings();
            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddTransient<IShowService, ShowService>();
            services.AddTransient<IShowCastProvider, ShowsCastProvider>();
            services.AddTransient<IShowScrapperService, ShowScrapperService>();
            services.AddPersistence(_configuration);

            services.AddSingleton<IShowContext, ShowContext>()
                .AddSingleton<IShowRepository, ShowRepository>();

            if (_configuration.GetValue<bool>("Scheduling:Enabled"))
            {
                services.AddConfig<ScheduleConfig>(_configuration, "Scheduling");
                services.AddHostedService<ShowScrapperScheduledService>();
            }

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IAsyncPolicy<HttpResponseMessage> GetHttpPolicies()
        {
            var randomExtraWait = new Random();

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => r.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                                      + TimeSpan.FromMilliseconds(randomExtraWait.Next(0, 100)));
        }
    }
}