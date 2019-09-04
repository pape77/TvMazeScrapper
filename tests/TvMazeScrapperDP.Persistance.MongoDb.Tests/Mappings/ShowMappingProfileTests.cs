using AutoMapper;
using TvMazeScrapperDP.Persistance.MongoDb.Mapping;
using Xunit;

namespace TvMazeScrapperDp.Persistance.MongoDb.Tests.Mappings
{
    public class ShowMappingProfileTests
    {
        private readonly MapperConfiguration _config;

        public ShowMappingProfileTests()
        {
            _config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ShowMappingProfile>();
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