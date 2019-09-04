using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Refit;
using TvMaze.Client;
using TvMaze.Client.Models;
using TvMazeScrapperDp.Core.Services.Contracts;
using Cast = TvMazeScrapperDp.Core.Models.Cast;
using Show = TvMazeScrapperDp.Core.Models.Show;
using ShowFromClient = TvMaze.Client.Models.Show;

namespace TvMazeScrapperDp.Core.Services
{
    public class ShowsCastProvider : IShowCastProvider
    {
        private readonly ITvMazeClient _tvMazeClient;
        private readonly IMapper _mapper;

        public ShowsCastProvider(ITvMazeClient tvMazeClient, IMapper mapper)
        {
            _tvMazeClient = tvMazeClient;
            _mapper = mapper;
        }

        public async Task<Show> FillInShowCastAsync(ShowFromClient showFromClient, CancellationToken cancellationToken)
        {
            var castResponse = await _tvMazeClient.GetCastAsync(showFromClient.Id, cancellationToken);

                await castResponse.EnsureSuccessStatusCodeAsync();

                IList<Person> persons = null;

                if (castResponse.Content != null)
                {
                    persons = castResponse.Content.OrderByDescending(x => x.Person.Birthday).Select(x => x.Person).ToList();
                }

                return new Show {Id = showFromClient.Id, Name = showFromClient.Name, Cast = persons?.Select(_mapper.Map<Cast>).ToList()};
            }
    }
}