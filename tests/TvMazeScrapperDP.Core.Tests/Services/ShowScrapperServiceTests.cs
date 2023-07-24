using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;
using TvMaze.Client;
using TvMazeScrapperDp.Core.Models;
using TvMazeScrapperDp.Core.Services;
using TvMazeScrapperDp.Core.Services.Contracts;
using Xunit;
using ShowFromClient = TvMaze.Client.Models.Show;

namespace Tv.MazeScrapperDP.Core.Tests.Services
{
    public class ShowScrapperServiceTests
    {
        private readonly Mock<ITvMazeClient> _tvMazeClientMock;
        private readonly Mock<IShowRepository> _showRepositoryMock;
        private readonly Mock<IShowCastProvider> _showCastProviderMock;
        private readonly ShowScrapperService _showScrapperService;

        public ShowScrapperServiceTests()
        {
            var fixture = new Fixture();
            var loggerMock = new Mock<ILogger<ShowScrapperService>>();

            _showRepositoryMock = new Mock<IShowRepository>();
            _showRepositoryMock.Setup(srm => srm.GetMaxIdAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            var shows = fixture.CreateMany<ShowFromClient>(250);

            _tvMazeClientMock = new Mock<ITvMazeClient>();
            _tvMazeClientMock.Setup(tvmc => tvmc.GetShowsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApiResponse<IEnumerable<ShowFromClient>>(new HttpResponseMessage(HttpStatusCode.OK), shows, new RefitSettings()));
            _tvMazeClientMock.Setup(tvmcm => tvmcm.GetShowsAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApiResponse<IEnumerable<ShowFromClient>>(new HttpResponseMessage(HttpStatusCode.NotFound), null, new RefitSettings()));

            _showCastProviderMock = new Mock<IShowCastProvider>();

            var show = fixture.Create<Show>();

            _showCastProviderMock.Setup(scpm => scpm.FillInShowCastAsync(It.IsAny<ShowFromClient>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(show));

            _showScrapperService = new ShowScrapperService(_tvMazeClientMock.Object, _showRepositoryMock.Object,
                _showCastProviderMock.Object, loggerMock.Object);
        }

        [Fact(DisplayName = "Scrapper calls what it should, accordingly and saves shows in repository")]
        public async Task Test01()
        {
            await _showScrapperService.ScrapAsync(CancellationToken.None);

            VerifyMocks(2, 3, 500);
        }

        [Fact(DisplayName = "Scrapper picks up from the  last page it had for scrapping again, according to the last max persisted id")]
        public async Task Test02()
        {
            _showRepositoryMock.Setup(srm => srm.GetMaxIdAsync(It.IsAny<CancellationToken>())).ReturnsAsync(249);

            await _showScrapperService.ScrapAsync(CancellationToken.None);

            VerifyMocks(1,2,250);
        }

        private void VerifyMocks(int timesShowRepository, int timesMazeClient, int timesShowCastProvider)
        {
            _tvMazeClientMock.Verify(tvmc => tvmc.GetShowsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(timesMazeClient));
            _tvMazeClientMock.VerifyNoOtherCalls();

            _showCastProviderMock.Verify(scpm => scpm.FillInShowCastAsync(It.IsAny<ShowFromClient>(), It.IsAny<CancellationToken>()),
                Times.AtLeast(timesShowCastProvider));

            _showCastProviderMock.VerifyNoOtherCalls();

            _showRepositoryMock.Verify(srm => srm.GetMaxIdAsync(It.IsAny<CancellationToken>()), Times.Once);

            _showRepositoryMock.Verify(srm => srm.SaveAsync(It.IsAny<IEnumerable<Show>>(), It.IsAny<CancellationToken>()),
                Times.Exactly(timesShowRepository));

            _showRepositoryMock.VerifyNoOtherCalls();
        }
    }
}