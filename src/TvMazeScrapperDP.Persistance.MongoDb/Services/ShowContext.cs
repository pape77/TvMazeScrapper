using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TvMazeScrapperDP.Persistance.MongoDb.Model;
using TvMazeScrapperDP.Persistance.MongoDb.Services.Contracts;

namespace TvMazeScrapperDP.Persistance.MongoDb.Services
{
    public class ShowContext : IShowContext
    {
        private readonly IMongoDatabase _db;
        private readonly MongoDbConfig _config;

        public ShowContext(IMongoDatabase db, IOptions<MongoDbConfig> config)
        {
            _db = db;
            _config = config.Value;
        }

        public IMongoCollection<Show> Show =>
            _db.GetCollection<Show>(_config.Collection);
    }
}