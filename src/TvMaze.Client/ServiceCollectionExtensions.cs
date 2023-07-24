using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Refit;
using TvMaze.Client.Options;

namespace TvMaze.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTvMazeClient(this IServiceCollection services, TvMazeClientOptions tvMazeClientOptions)
        {
            services
                .AddRefitClient<ITvMazeClient>()
                .AddPolicyHandler(GetHttpPolicies())
                .ConfigureHttpClient(config =>
                {
                    config.BaseAddress = tvMazeClientOptions.BaseUri;
                });
                
            return services;
        }
        
        private static IAsyncPolicy<HttpResponseMessage> GetHttpPolicies()
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