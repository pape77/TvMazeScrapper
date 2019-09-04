using System.Collections.Generic;

namespace TvMaze.Client.Models
{
    public class Show
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Cast> Cast { get; set; }
    }
}