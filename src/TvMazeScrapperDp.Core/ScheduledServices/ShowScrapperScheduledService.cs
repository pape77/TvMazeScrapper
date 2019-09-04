using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCrontab;
using NodaTime;
using TvMazeScrapperDp.Core.Models;
using TvMazeScrapperDp.Core.Services.Contracts;

namespace TvMazeScrapperDp.Core.ScheduledServices
{
    public class ShowScrapperScheduledService : ScheduledServiceBase<ShowScrapperScheduledService>
    {
        private DateTime _nextRun;
        private DateTime _now;
        private readonly CrontabSchedule _schedule;
        private readonly ScheduleConfig _scheduleConfig;
        private readonly IClock _clock;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ShowScrapperScheduledService(ILogger<ShowScrapperScheduledService> logger, IClock clock,
            IOptions<ScheduleConfig> config, IServiceScopeFactory serviceScopeFactory) : base(logger)
        {
            _scheduleConfig = config.Value;
            _clock = clock;
            _serviceScopeFactory = serviceScopeFactory;

            _schedule = CrontabSchedule.Parse(_scheduleConfig.CronExpression);
            _nextRun = _clock.GetCurrentInstant().ToDateTimeUtc();
        }

        public override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            _now = _clock.GetCurrentInstant().ToDateTimeUtc();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                Logger.LogInformation("Starting Service {0} for scrapping and persisting Shows...", nameof(IShowScrapperService));
                var showScrapperService = scope.ServiceProvider.GetRequiredService<IShowScrapperService>();
                await showScrapperService.ScrapAsync(cancellationToken);
            }
        }

        public override async Task AfterWorkDoneDelay()
        {
            _nextRun = _schedule.GetNextOccurrence(_now);
            await Task.Delay(_nextRun - _now);
        }

        public override Task OnErrorDelay() => Task.Delay(TimeSpan.FromSeconds(_scheduleConfig.OnErrorDelay));
    }
}