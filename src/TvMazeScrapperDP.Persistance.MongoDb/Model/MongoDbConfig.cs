using System.ComponentModel.DataAnnotations;

namespace TvMazeScrapperDP.Persistance.MongoDb.Model
{
    public class MongoDbConfig
    {
        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string Database { get; set; }

        [Required]
        public string Collection { get; set; }
    }
}