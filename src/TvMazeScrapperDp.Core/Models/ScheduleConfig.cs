using System.ComponentModel.DataAnnotations;

namespace TvMazeScrapperDp.Core.Models
{
    public class ScheduleConfig
    {
        [Required]
        public string CronExpression { get; set; }

        [Required]
        public int OnErrorDelay { get; set; }

        public bool Enabled { get; set; }
    }
}