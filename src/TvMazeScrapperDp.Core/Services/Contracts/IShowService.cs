using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScrapperDp.Core.Models;

namespace TvMazeScrapperDp.Core.Services.Contracts
{
    public interface IShowService
    {
        Task<IEnumerable<Show>> GetAllPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<Show?> GetShowAsync(int id, CancellationToken cancellationToken);
    }
}