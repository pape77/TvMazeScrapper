using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TvMazeScrapperDp.Core.ScheduledServices
{
    public abstract class ScheduledServiceBase<T> : BackgroundService where T : class
    {
        protected readonly ILogger<T> Logger;

        protected ScheduledServiceBase(ILogger<T> logger)
        {
            Logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Started {Service} at {Time}.", typeof(T).Name, DateTime.UtcNow);

            cancellationToken.Register(() =>
                Logger.LogInformation("Stopped {Service} at {Time}.", typeof(T).Name, DateTime.UtcNow));

            do{
                try
                {
                    await DoWorkAsync(cancellationToken);
                    await AfterWorkDoneDelay();
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message);
                    Logger.LogError(e.StackTrace);
                    await OnErrorDelay();
                }
            }
            while (!cancellationToken.IsCancellationRequested);
        }

        public virtual Task OnErrorDelay() => Task.Delay(TimeSpan.FromSeconds(15));

        public virtual Task AfterWorkDoneDelay() => Task.Delay(TimeSpan.FromDays(1));

        public abstract Task DoWorkAsync(CancellationToken cancellationToken);
    }
}