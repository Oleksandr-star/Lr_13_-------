using OpenTelemetry;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Lb13.Exporters
{
    public class FirstExporter : BaseExporter<Activity>
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _uri;

        public FirstExporter(string uri)
        {
            _httpClient = new HttpClient();
            _uri = new Uri(uri);
        }

        public override ExportResult Export(in Batch<Activity> batch)
        {
            foreach (var activity in batch)
            {
                if (SendToCustomBackend(activity).Result)
                {
                    return ExportResult.Success;
                }
            }
            return ExportResult.Failure;
        }

        private async Task<bool> SendToCustomBackend(Activity activity)
        {
            try
            {
                var payload = JsonSerializer.Serialize(new
                {
                    id = activity.Id,
                    traceId = activity.TraceId.ToHexString(),
                    spanId = activity.SpanId.ToHexString(),
                    parentId = activity.ParentId,
                    operationName = activity.DisplayName,
                    startTime = activity.StartTimeUtc,
                    duration = activity.Duration,
                    tags = activity.Tags
                });

                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_uri, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error: {ex.Message}");
                return false;
            }
        }
    }
}
