using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TvMazeScrapperDp.Core.Models;

namespace TvMazeScrapperDp.Core.Services.Contracts
{
    public interface IShowService
    {
        Task<IEnumerable<Show>> GetAllPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<Maybe<Show>> GetShowAsync(int id, CancellationToken cancellationToken);
    }
}