using System;
using System.ComponentModel.DataAnnotations;

namespace TvMaze.Client.Options
{
    public class TvMazeClientOptions
    {
        [Required]
        public Uri BaseUri { get; set; }
    }
}