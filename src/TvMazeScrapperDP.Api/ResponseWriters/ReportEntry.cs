using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TvMazeScrapperDP.Api.ResponseWriters
{
    public class ReportEntry
    {
        public string Name { get; set; }

        [JsonConverter(typeof (JsonStringEnumConverter))]
        public HealthStatus Status { get; set; }

        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Exception { get; set; }
    }
}