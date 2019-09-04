using AutoMapper;
using TvMazeScrapperDp.Core.Models;

namespace TvMazeScrapperDP.Persistance.MongoDb.Mapping
{
    public class ShowMappingProfile : Profile
    {
        public ShowMappingProfile()
        {
            CreateMap<Show, Model.Show>().ReverseMap();
            CreateMap<Cast, Model.Cast>().ReverseMap();
        }
    }
}