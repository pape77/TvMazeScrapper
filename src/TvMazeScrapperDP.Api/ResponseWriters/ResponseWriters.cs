using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace TvMazeScrapperDP.Api.ResponseWriters
{
    public class ResponseWriters
    {
        internal static async Task WriteFullJsonReportAsync(
            HttpContext httpContext,
            HealthReport healthReport)
        {
            var text = JsonConvert.SerializeObject(CreateReport(healthReport));
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(text, Encoding.UTF8, new CancellationToken());
        }

        private static Report CreateReport(HealthReport healthReport)
        {
            return new Report()
            {
                Status = healthReport.Status,
                Checks = healthReport.Entries.Select(x => new ReportEntry()
                    {
                        Name = x.Key,
                        Status = x.Value.Status,
                        Description = x.Value.Description,
                        Exception = x.Value.Exception?.Message
                    })
            };
        }
    }
}