using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TvMazeScrapperDP.Api.ResponseWriters
{
    public class ReportEntry
    {
        public string Name { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public HealthStatus Status { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Exception { get; set; }
    }
}