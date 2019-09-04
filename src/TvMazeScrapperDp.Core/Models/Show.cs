using System.Collections.Generic;

namespace TvMazeScrapperDp.Core.Models
{
    public class Show
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Cast> Cast { get; set; }
    }
}