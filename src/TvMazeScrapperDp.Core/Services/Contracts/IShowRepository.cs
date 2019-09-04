using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScrapperDp.Core.Models;

namespace TvMazeScrapperDp.Core.Services.Contracts
{
    public interface IShowRepository
    {
        Task<long> CountAsync(CancellationToken cancellationToken);

        Task<int> GetMaxIdAsync(CancellationToken cancellationToken);
        Task<Show> FindAsync(int id, CancellationToken cancellationToken);

        Task SaveAsync(IEnumerable<Show> shows, CancellationToken cancellationToken);

        Task<IEnumerable<Show>> GetPaginatedAsync(int page, int pageSize,
            CancellationToken cancellationToken);
    }
}