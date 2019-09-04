using MongoDB.Driver;

namespace TvMazeScrapperDP.Persistance.MongoDb.Services.Contracts
{
    public interface IMongoDbClient
    {
        IMongoDatabase GetDatabase(string database);
    }
}