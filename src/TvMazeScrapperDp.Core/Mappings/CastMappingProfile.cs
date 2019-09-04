using AutoMapper;
using TvMaze.Client.Models;
using Cast = TvMazeScrapperDp.Core.Models.Cast;

namespace TvMazeScrapperDp.Core.Mappings
{
    public class CastMappingProfile : Profile
    {
        public CastMappingProfile()
        {
            CreateMap<Person, Cast>();
        }
    }
}