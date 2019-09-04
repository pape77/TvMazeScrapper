using System.Security.Authentication;
using MongoDB.Driver;
using TvMazeScrapperDP.Persistance.MongoDb.Services.Contracts;

namespace TvMazeScrapperDP.Persistance.MongoDb.Services
{
    public class MongoDbClientFactory : IMongoDbClientFactory
    {
        public IMongoDbClient Create(string connectionString)
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);

            settings.SslSettings = new SslSettings {EnabledSslProtocols = SslProtocols.Tls12};
            settings.RetryWrites = true;

            return new MongoDbClient(settings);
        }
    }
}