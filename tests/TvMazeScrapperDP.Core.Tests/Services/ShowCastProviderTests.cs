using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Moq;
using Refit;
using TvMaze.Client;
using TvMaze.Client.Models;
using TvMazeScrapperDp.Core.Services;
using Xunit;
using Cast = TvMazeScrapperDp.Core.Models.Cast;
using CastFromClient = TvMaze.Client.Models.Cast;

namespace Tv.MazeScrapperDP.Core.Tests.Services
{
    public class ShowCastProviderTests
    {
        private readonly Mock<ITvMazeClient> _tvMazeClientMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Show _inputShow;
        private readonly ShowsCastProvider _showsCastProvider;

        public ShowCastProviderTests()
        {
            var fixture = new Fixture();

            _inputShow = fixture.Build<Show>()
                .Without(s => s.Cast)
                .Create();

            var castList = fixture.CreateMany<CastFromClient>(2).ToList();
            castList[0].Person.Birthday = "1988-16-10";
            castList[1].Person.Birthday = "1986-01-01";

            _tvMazeClientMock = new Mock<ITvMazeClient>();
            _tvMazeClientMock.Setup(tvmcm => tvmcm.GetCastAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApiResponse<IEnumerable<CastFromClient>>(new HttpResponseMessage(HttpStatusCode.OK), castList, new RefitSettings()));

            _mapperMock = new Mock<IMapper>();
            _mapperMock.SetupSequence(m => m.Map<Cast>(It.IsAny<Person>()))
                .Returns(new Cast
                {
                    Id = castList.First().Person.Id,
                    Birthday = castList.First().Person.Birthday,
                    Name = castList.First().Person.Name
                })
                .Returns(new Cast
                {
                    Id = castList[1].Person.Id,
                    Birthday = castList[1].Person.Birthday,
                    Name = castList[1].Person.Name
                });

            _showsCastProvider = new ShowsCastProvider(_tvMazeClientMock.Object, _mapperMock.Object);
        }

        [Fact(DisplayName = "Returns show with cast ordered by Birthday desc")]
        public async Task Test01()
        {
            var resultShow = await _showsCastProvider.FillInShowCastAsync(_inputShow, CancellationToken.None);

            Assert.NotNull(resultShow.Cast);
            Assert.Equal(2, resultShow.Cast.Count);
            Assert.Equal("1988-16-10", resultShow.Cast.First().Birthday);

            _mapperMock.Verify(m => m.Map<Cast>(It.IsAny<Person>()), Times.Exactly(2));
            VerifyMocks();
        }

        [Fact(DisplayName = "Returns null Cast if client returned a null cast response")]
        public async Task Test02()
        {
            _tvMazeClientMock.Setup(tvmcm => tvmcm.GetCastAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApiResponse<IEnumerable<CastFromClient>>(new HttpResponseMessage(HttpStatusCode.OK), null, new RefitSettings()));

            var resultshow = await _showsCastProvider.FillInShowCastAsync(_inputShow, CancellationToken.None);

            Assert.Null(resultshow.Cast);
            
            VerifyMocks();
        }

        private void VerifyMocks()
        {
            _tvMazeClientMock.Verify(tvmcm => tvmcm.GetCastAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
            _tvMazeClientMock.VerifyNoOtherCalls();

            _mapperMock.VerifyNoOtherCalls();
        }
    }
}