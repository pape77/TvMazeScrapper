using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using TvMazeScrapperDp.Core.Models;
using TvMazeScrapperDp.Core.Services;
using TvMazeScrapperDp.Core.Services.Contracts;
using Xunit;

namespace Tv.MazeScrapperDP.Core.Tests.Services
{
    public class ShowServiceTests
    {
        private readonly Mock<IShowRepository> _showRepositoryMock;
        private readonly Show _show;
        private readonly ShowService _showService;

        public ShowServiceTests()
        {
            var fixture = new Fixture();
            _show = fixture.Create<Show>();
            _showRepositoryMock = new Mock<IShowRepository>();

            _showRepositoryMock.Setup(srm => srm.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Show>{_show});

            _showRepositoryMock.Setup(srm => srm.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(_show);
            _showRepositoryMock.Setup(srm => srm.FindAsync(100, It.IsAny<CancellationToken>())).ReturnsAsync((Show) null);

            _showService = new ShowService(_showRepositoryMock.Object);
        }

        [Fact(DisplayName = "Calls pagination method on show repository and returns Enumerable of shows")]
        public async Task  Test01()
        {
            var pageSize = GetRandomNumber();
            var page = GetRandomNumber();

            var result = await _showService.GetAllPaginatedAsync(page, pageSize, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(result.Single(), _show);
            _showRepositoryMock.Verify(srm => srm.GetPaginatedAsync(page, pageSize, CancellationToken.None), Times.Once);
            _showRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "Calls FindAsync method on show repository and returns show if found")]
        public async Task  Test02()
        {
            var id = GetRandomNumber();

            var result = await _showService.GetShowAsync(id, CancellationToken.None);

            Assert.True(result.HasValue);
            Assert.Equal(result.Value, _show);
            _showRepositoryMock.Verify(srm => srm.FindAsync(id, CancellationToken.None), Times.Once);
            _showRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "Calls FindAsync method on show repository and returns Maybe.None if show is not found")]
        public async Task  Test03()
        {
            var result = await _showService.GetShowAsync(100, CancellationToken.None);

            Assert.True(result.HasNoValue);
            _showRepositoryMock.Verify(srm => srm.FindAsync(100, CancellationToken.None), Times.Once);
            _showRepositoryMock.VerifyNoOtherCalls();
        }

        private int GetRandomNumber(int maxValue = 99)
        {
            var random = new Random(maxValue);

            return random.Next();
        }
    }
}