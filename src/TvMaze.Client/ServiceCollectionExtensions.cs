using Microsoft.Extensions.DependencyInjection;
using Refit;
using TvMaze.Client.Models;

namespace TvMaze.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddTvMazeClient(this IServiceCollection services, TvMazeClientConfig tvMazeClientConfig)
        {
            return services.AddRefitClient<ITvMazeClient>().ConfigureHttpClient(config =>
            {
                config.BaseAddress = tvMazeClientConfig.BaseUri;
            });
        }
    }
}