using AutoMapper;
using TvMazeScrapperDp.Core.Mappings;
using Xunit;

namespace Tv.MazeScrapperDP.Core.Tests.Mappings
{
    public class CastMappingProfileTests
    {
        private readonly MapperConfiguration _config;

        public CastMappingProfileTests()
        {
            _config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CastMappingProfile>();
            });

            _config.CompileMappings();
        }

        [Fact(DisplayName = "Maps model correctly")]
        public void Test01()
        {
            _config.AssertConfigurationIsValid();
        }
    }
}