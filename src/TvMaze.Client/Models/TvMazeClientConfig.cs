using System;
using System.ComponentModel.DataAnnotations;

namespace TvMaze.Client.Models
{
    public class TvMazeClientConfig
    {
        [Required]
        public Uri BaseUri { get; set; }
    }
}