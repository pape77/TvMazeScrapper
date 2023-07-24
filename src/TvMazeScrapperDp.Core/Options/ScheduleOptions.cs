using System.ComponentModel.DataAnnotations;

namespace TvMazeScrapperDp.Core.Options
{
    public class ScheduleOptions
    {
        [Required]
        public string CronExpression { get; set; }

        [Required]
        public int OnErrorDelay { get; set; }

        public bool Enabled { get; set; }
    }
}