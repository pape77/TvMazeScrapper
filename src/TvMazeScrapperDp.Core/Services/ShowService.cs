using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TvMazeScrapperDp.Core.Models;
using TvMazeScrapperDp.Core.Services.Contracts;

namespace TvMazeScrapperDp.Core.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;

        public ShowService(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        public async Task<IEnumerable<Show>> GetAllPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _showRepository.GetPaginatedAsync(page, pageSize, cancellationToken);
        }

        public async Task<Maybe<Show>> GetShowAsync(int id, CancellationToken cancellationToken)
        {
            return await _showRepository.FindAsync(id, cancellationToken);
        }
    }
}