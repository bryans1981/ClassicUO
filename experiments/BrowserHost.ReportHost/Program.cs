using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "browser-spike";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        CorsPolicy,
        policy => policy
            .WithOrigins("http://localhost:5099")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

var app = builder.Build();
app.UseCors(CorsPolicy);

string reportsRoot = Path.Combine(app.Environment.ContentRootPath, "..", "..", "docs", "test-results");
Directory.CreateDirectory(reportsRoot);

app.MapGet("/health", () => Results.Ok(new { ok = true }));

app.MapPost(
    "/api/self-test-reports",
    async (BrowserLocalSelfTestReportRequest request) =>
    {
        if (string.IsNullOrWhiteSpace(request.Json))
        {
            return Results.BadRequest(new BrowserLocalSelfTestReportResponse
            {
                Saved = false,
                Error = "Report JSON is required."
            });
        }

        DateTimeOffset now = DateTimeOffset.UtcNow;
        string timestamp = now.ToString("yyyyMMdd-HHmmss");
        string latestPath = Path.Combine(reportsRoot, "browser-self-test-latest.json");
        string archivePath = Path.Combine(reportsRoot, $"browser-self-test-{timestamp}.json");

        await File.WriteAllTextAsync(latestPath, request.Json);
        await File.WriteAllTextAsync(archivePath, request.Json);

        return Results.Ok(new BrowserLocalSelfTestReportResponse
        {
            Saved = true,
            LatestPath = latestPath,
            ArchivePath = archivePath,
            SavedAtUtc = now
        });
    }
);

app.Run();

public sealed class BrowserLocalSelfTestReportRequest
{
    public string Json { get; set; } = string.Empty;
}

public sealed class BrowserLocalSelfTestReportResponse
{
    public bool Saved { get; set; }
    public string LatestPath { get; set; } = string.Empty;
    public string ArchivePath { get; set; } = string.Empty;
    public DateTimeOffset SavedAtUtc { get; set; }
    public string Error { get; set; } = string.Empty;
}
