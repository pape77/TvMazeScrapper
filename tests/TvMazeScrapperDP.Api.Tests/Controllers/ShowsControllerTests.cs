using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TvMazeScrapperDP.Api.Controllers;
using TvMazeScrapperDP.Api.Models;
using TvMazeScrapperDp.Core.Models;
using TvMazeScrapperDp.Core.Services.Contracts;
using Xunit;

namespace TvMazeScrapperDP.Api.Tests.Controllers
{
    public class ShowsControllerTests
    {
        private readonly ShowsController _showController;
        private readonly IEnumerable<Show> _expectedPaginatedResponse;
        private readonly Show _expectedShow;
        private readonly Mock<IShowService> _showServiceMock;

        public ShowsControllerTests()
        {
            var fixture = new Fixture();
            _expectedPaginatedResponse = fixture.CreateMany<Show>(3);
            _expectedShow = fixture.Create<Show>();
            _expectedShow.Id = 1;

            _showServiceMock = new Mock<IShowService>();

            _showServiceMock.Setup(ssm => ssm.GetShowAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_expectedShow);

            _showServiceMock.Setup(ssm => ssm.GetAllPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_expectedPaginatedResponse);

            _showController = new ShowsController(_showServiceMock.Object);
        }

        [Fact(DisplayName = "Retrieve expected paginated response as value")]
        public async Task Test01()
        {
           var result = await _showController.Get(new PaginatedRequest{Page = 1, PageSize = 20}, CancellationToken.None);

           Assert.IsType<OkObjectResult>(result);
           var okObjectResult = (OkObjectResult)result;

           Assert.Equal(_expectedPaginatedResponse, okObjectResult.Value);
        }

        [Fact(DisplayName = "Retrieves expected show as value")]
        public async Task Test02()
        {
            var result = await _showController.Get(1, CancellationToken.None);

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;

            Assert.Equal(_expectedShow, okObjectResult.Value);
        }

        [Fact(DisplayName = "Retrieves expected 404 when show is not found")]
        public async Task Test03()
        {
            _showServiceMock.Setup(ssm => ssm.GetShowAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Show)null);

            var result = await _showController.Get(1, CancellationToken.None);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}