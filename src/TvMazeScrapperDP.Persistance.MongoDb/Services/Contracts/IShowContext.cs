using MongoDB.Driver;
using TvMazeScrapperDP.Persistance.MongoDb.Model;

namespace TvMazeScrapperDP.Persistance.MongoDb.Services.Contracts
{
    public interface IShowContext
    {
        IMongoCollection<Show> Show { get; }
    }
}