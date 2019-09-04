namespace TvMazeScrapperDP.Persistance.MongoDb.Services.Contracts
{
    public interface IMongoDbClientFactory
    {
        IMongoDbClient Create(string connectionString);
    }
}