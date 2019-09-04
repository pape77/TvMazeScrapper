using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using TvMaze.Client.Models;

namespace TvMaze.Client
{
    public interface ITvMazeClient
    {
        [Get("/shows")]
        Task<ApiResponse<IEnumerable<Show>>> GetShowsAsync(int page, CancellationToken cancellationToken);

        [Get("/shows/{id}/cast")]
        Task<ApiResponse<IEnumerable<Cast>>> GetCastAsync(int id, CancellationToken cancellationToken);
    }
}