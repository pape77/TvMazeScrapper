using System.Threading;
using System.Threading.Tasks;
using TvMazeScrapperDp.Core.Models;
using ShowFromClient = TvMaze.Client.Models.Show;

namespace TvMazeScrapperDp.Core.Services.Contracts
{
    public interface IShowCastProvider
    {
        Task<Show> FillInShowCastAsync(ShowFromClient showFromClient, CancellationToken cancellationToken);
    }
}