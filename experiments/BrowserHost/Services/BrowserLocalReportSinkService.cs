using System.Net.Http.Json;
using System.Text.Json;

namespace BrowserHost.Services;

public sealed class BrowserLocalReportSinkService
{
    private readonly HttpClient _httpClient;

    public BrowserLocalReportSinkService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5100")
        };
    }

    public async ValueTask<BrowserLocalReportSaveResult> SaveSelfTestReportAsync(string json)
    {
        BrowserLocalSelfTestReportRequest request = new()
        {
            Json = json
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/self-test-reports", request);

        if (!response.IsSuccessStatusCode)
        {
            return new BrowserLocalReportSaveResult
            {
                Saved = false,
                Error = $"Report save failed with status {(int)response.StatusCode}."
            };
        }

        BrowserLocalReportSaveResult? result = await response.Content.ReadFromJsonAsync<BrowserLocalReportSaveResult>();

        return result ?? new BrowserLocalReportSaveResult
        {
            Saved = false,
            Error = "Report save returned no result."
        };
    }

    public ValueTask<BrowserLocalReportSaveResult> SaveSelfTestFailureAsync(string message, Exception exception)
    {
        BrowserLocalSelfTestFailureReportRequest request = new()
        {
            GeneratedAtUtc = DateTimeOffset.UtcNow,
            ReportMode = "compact-browser-runtime-v1",
            RuntimeChainMarker = BrowserSelfTestReportService.RuntimeChainMarker,
            Summary = message,
            Error = exception.ToString()
        };

        return SaveSelfTestReportAsync(JsonSerializer.Serialize(request));
    }
}

public sealed class BrowserLocalSelfTestReportRequest
{
    public string Json { get; set; } = string.Empty;
}

public sealed class BrowserLocalSelfTestFailureReportRequest
{
    public DateTimeOffset GeneratedAtUtc { get; set; }
    public string ReportMode { get; set; } = string.Empty;
    public string RuntimeChainMarker { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserLocalReportSaveResult
{
    public bool Saved { get; set; }
    public string LatestPath { get; set; } = string.Empty;
    public string ArchivePath { get; set; } = string.Empty;
    public DateTimeOffset SavedAtUtc { get; set; }
    public string Error { get; set; } = string.Empty;
}
