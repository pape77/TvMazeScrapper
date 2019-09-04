using System.Threading;
using System.Threading.Tasks;

namespace TvMazeScrapperDp.Core.Services.Contracts
{
    public interface IShowScrapperService
    {
        Task ScrapAsync(CancellationToken cancellationToken);
    }
}