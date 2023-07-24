using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TvMazeScrapperDP.Api.ResponseWriters
{
    public class Report
    {
        [JsonConverter(typeof (JsonStringEnumConverter))]
        public HealthStatus Status { get; set; }

        public IEnumerable<ReportEntry> Checks { get; set; }
    }
}