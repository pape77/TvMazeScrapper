using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MoreLinq;
using TvMaze.Client;
using TvMazeScrapperDp.Core.Services.Contracts;
using Show = TvMazeScrapperDp.Core.Models.Show;
using ShowFromClient = TvMaze.Client.Models.Show;

namespace TvMazeScrapperDp.Core.Services
{
    public class ShowScrapperService : IShowScrapperService
    {
        private readonly ILogger<ShowScrapperService> _logger;
        private readonly ITvMazeClient _tvMazeClient;
        private readonly IShowRepository _showRepository;
        private readonly IShowCastProvider _showCastProvider;
        private const double MaxResultsPerPage = 250;

        public ShowScrapperService(ITvMazeClient tvMazeClient, IShowRepository showRepository,
            IShowCastProvider showCastProvider, ILogger<ShowScrapperService> logger)
        {
            _tvMazeClient = tvMazeClient;
            _showRepository = showRepository;
            _logger = logger;
            _showCastProvider = showCastProvider;
        }

        public async Task ScrapAsync(CancellationToken cancellationToken)
        {
            var maxPersistedId = await _showRepository.GetMaxIdAsync(cancellationToken);
            var nextMissingPage = (int)Math.Floor((maxPersistedId+1) / MaxResultsPerPage);
            var newShowsPersistedCount = 0;

            for (var page = nextMissingPage;; page++)
            {
                var showsResponse = await _tvMazeClient.GetShowsAsync(page, cancellationToken);

                if (showsResponse.StatusCode == HttpStatusCode.NotFound) //there're no more shows on a 404, according to docs
                {
                    _logger.LogInformation("Finalizing Scrapping with a total of {0} pages retrieved", page);
                    _logger.LogInformation("Persisted {0} new shows", newShowsPersistedCount);
                    break;
                }

                await showsResponse.EnsureSuccessStatusCodeAsync();

                var retrievedNewShows = showsResponse.Content;

                var list = await PopulateShowList(retrievedNewShows, cancellationToken);

                if (list.Any())
                {
                    await _showRepository.SaveAsync(list, cancellationToken);
                }

                newShowsPersistedCount += list.Count;
            }
        }

        private async Task<List<Show>> PopulateShowList(IEnumerable<ShowFromClient> retrievedNewShows, CancellationToken cancellationToken)
        {
            var list = new List<Show>();

            foreach (var retrievedNewShowsBatch in retrievedNewShows.ToList().Batch(10))
            {
                var fillShowWithCastTasks = retrievedNewShowsBatch.Select(s => _showCastProvider.FillInShowCastAsync(s, cancellationToken));
                await Task.WhenAll(fillShowWithCastTasks);
                list.AddRange(fillShowWithCastTasks.Select(s => s.Result));
            }

            return list;
        }
    }
}