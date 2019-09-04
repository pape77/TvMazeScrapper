using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TvMazeScrapperDP.Api.ResponseWriters
{
    public class Report
    {
        [JsonConverter(typeof (StringEnumConverter))]
        public HealthStatus Status { get; set; }

        public IEnumerable<ReportEntry> Checks { get; set; }
    }
}