using MongoDB.Driver;
using TvMazeScrapperDP.Persistance.MongoDb.Services.Contracts;

namespace TvMazeScrapperDP.Persistance.MongoDb.Services
{
    public class MongoDbClient : IMongoDbClient
    {
        private readonly MongoClient _client;

        public MongoDbClient(MongoClientSettings settings)
        {
            _client = new MongoClient(settings);
        }

        public IMongoDatabase GetDatabase(string database) => _client.GetDatabase(database);
    }
}