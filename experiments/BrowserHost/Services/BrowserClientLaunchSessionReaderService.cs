using System.Text.Json;
using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientLaunchSessionReader
{
    ValueTask<BrowserClientLaunchSessionRead> ReadLaunchSessionAsync(string profileId = "default");
}

public sealed class BrowserClientLaunchSessionReaderService : IBrowserClientLaunchSessionReader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly BrowserStorageService _storageService;

    public BrowserClientLaunchSessionReaderService(BrowserStorageService storageService)
    {
        _storageService = storageService;
    }

    public async ValueTask<BrowserClientLaunchSessionRead> ReadLaunchSessionAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        string normalizedProfileId = NormalizeProfileId(profileId);
        string sessionPath = BrowserVirtualPaths.CacheFile($"startup/{normalizedProfileId}/launch-session.json");

        BrowserFileTextResult readResult = await _storageService.ReadTextAsync(sessionPath);
        BrowserClientLaunchSessionRead result = new()
        {
            ProfileId = normalizedProfileId,
            SessionPath = sessionPath,
            Exists = readResult.Exists
        };

        if (!readResult.Exists)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"No launch session found at {sessionPath}.";
            result.Error = readResult.Error;
            return result;
        }

        try
        {
            BrowserClientLaunchSession? session = JsonSerializer.Deserialize<BrowserClientLaunchSession>(readResult.Text, JsonOptions);

            if (session is null)
            {
                result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
                result.Summary = $"Launch session at {sessionPath} could not be deserialized.";
                result.Error = "Launch session deserialized to null.";
                return result;
            }

            result.ReadSucceeded = true;
            result.IsReady = session.IsReady;
            result.Length = readResult.Text.Length;
            result.Session = session;
            result.RequiredAssetCount = session.RequiredAssets.Length;
            result.CompletedSteps = session.CompletedSteps;
            result.TotalSteps = session.TotalSteps;
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = session.IsReady
                ? $"Launch session loaded for profile '{normalizedProfileId}'."
                : $"Launch session loaded but not ready for profile '{normalizedProfileId}'.";
            return result;
        }
        catch (Exception ex)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Launch session read failed for profile '{normalizedProfileId}'.";
            result.Error = ex.Message;
            return result;
        }
    }

    private static string NormalizeProfileId(string profileId)
    {
        if (string.IsNullOrWhiteSpace(profileId))
        {
            return "default";
        }

        char[] chars = profileId.Trim().Select(static c => char.IsLetterOrDigit(c) || c is '-' or '_' ? c : '-').ToArray();
        string normalized = new string(chars).Trim('-');
        return string.IsNullOrWhiteSpace(normalized) ? "default" : normalized;
    }
}

public sealed class BrowserClientLaunchSessionRead
{
    public string ProfileId { get; set; } = "default";
    public string SessionPath { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public bool IsReady { get; set; }
    public int Length { get; set; }
    public int RequiredAssetCount { get; set; }
    public int CompletedSteps { get; set; }
    public int TotalSteps { get; set; }
    public BrowserClientLaunchSession Session { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
