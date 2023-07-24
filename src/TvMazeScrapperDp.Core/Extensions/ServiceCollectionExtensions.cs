using System;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScrapperDp.Core.Options;
using TvMazeScrapperDp.Core.ScheduledServices;
using TvMazeScrapperDp.Core.Services;
using TvMazeScrapperDp.Core.Services.Contracts;

namespace TvMazeScrapperDp.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, Action<ScheduleOptions> configureOptions)
    {
        services.AddTransient<IShowService, ShowService>();
        services.AddTransient<IShowCastProvider, ShowsCastProvider>();
        services.AddTransient<IShowScrapperService, ShowScrapperService>();

        var scheduleOptions = new ScheduleOptions();
        configureOptions(scheduleOptions);
        services.Configure(configureOptions);
        
        if (scheduleOptions.Enabled)
        {
            services.AddHostedService<ShowScrapperScheduledService>();
        }
        return services;
    }
}