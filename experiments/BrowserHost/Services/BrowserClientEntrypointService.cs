using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientEntrypoint
{
    ValueTask<BrowserClientLaunchPlan> PrepareLaunchAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientEntrypointService : IBrowserClientEntrypoint
{
    private readonly IBrowserClientAssetService _browserClientAssetService;
    private readonly BrowserStorageService _storageService;

    public BrowserClientEntrypointService(
        IBrowserClientAssetService browserClientAssetService,
        BrowserStorageService storageService
    )
    {
        _browserClientAssetService = browserClientAssetService;
        _storageService = storageService;
    }

    public async ValueTask<BrowserClientLaunchPlan> PrepareLaunchAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        string normalizedProfileId = NormalizeProfileId(profileId);
        DateTimeOffset started = DateTimeOffset.UtcNow;

        BrowserClientBootstrapHandoff handoff = await _browserClientAssetService.GetBrowserBootstrapHandoffAsync(request);
        string settingsFilePath = BrowserVirtualPaths.ConfigFile("client.settings.json");
        string startupProfilePath = BrowserVirtualPaths.ProfileFile(normalizedProfileId, "profile.json");

        BrowserFilePreparation settingsPreparation = await EnsureJsonFileAsync(settingsFilePath, "{}");
        BrowserFilePreparation profilePreparation = await EnsureJsonFileAsync(startupProfilePath, "{}");

        BrowserClientStartupAsset[] startupAssets = handoff.Assets.Select(
            static asset => new BrowserClientStartupAsset
            {
                Id = asset.Id,
                Path = asset.Path,
                Exists = asset.Exists,
                ReadSucceeded = asset.ReadSucceeded,
                LoadedOnDemand = asset.LoadedOnDemand,
                UsedParsedCache = asset.UsedParsedCache,
                Length = asset.Length,
                IsRequired = true,
                Summary = asset.Summary
            }
        ).ToArray();

        BrowserClientLaunchAsset[] assets = startupAssets.Select(
            static asset => new BrowserClientLaunchAsset
            {
                Id = asset.Id,
                Path = asset.Path,
                Exists = asset.Exists,
                ReadSucceeded = asset.ReadSucceeded,
                IsRequired = asset.IsRequired,
                Length = asset.Length,
                Summary = asset.Summary
            }
        ).ToArray();

        BrowserClientLaunchIssue[] issues = assets
            .Where(static x => x.IsRequired && (!x.Exists || !x.ReadSucceeded))
            .Select(
                static x => new BrowserClientLaunchIssue
                {
                    Code = $"missing-{x.Id}",
                    Message = $"Required startup asset is not ready: {x.Path}"
                }
            )
            .ToArray();

        bool isReady = handoff.IsReady
            && settingsPreparation.Succeeded
            && profilePreparation.Succeeded
            && issues.Length == 0;

        return new BrowserClientLaunchPlan
        {
            IsReady = isReady,
            ProfileId = normalizedProfileId,
            AssetRootPath = BrowserVirtualPaths.AssetsRoot,
            ProfilesRootPath = BrowserVirtualPaths.ProfilesRoot,
            CacheRootPath = BrowserVirtualPaths.CacheRoot,
            ConfigRootPath = BrowserVirtualPaths.ConfigRoot,
            SettingsFilePath = settingsFilePath,
            StartupProfilePath = startupProfilePath,
            SettingsPrepared = settingsPreparation.Succeeded,
            StartupProfilePrepared = profilePreparation.Succeeded,
            ReadyAssetCount = handoff.ReadyAssetCount,
            CacheHits = handoff.CacheHits,
            TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds,
            Assets = assets,
            Issues = issues,
            Summary = BuildSummary(isReady, normalizedProfileId, issues.Length, handoff.Summary)
        };
    }

    private async ValueTask<BrowserFilePreparation> EnsureJsonFileAsync(string path, string defaultContents)
    {
        bool exists = await _storageService.FileExistsAsync(path);

        if (exists)
        {
            return new BrowserFilePreparation
            {
                Path = path,
                Succeeded = true,
                Created = false
            };
        }

        BrowserFileWriteResult writeResult = await _storageService.WriteTextAsync(path, defaultContents);

        return new BrowserFilePreparation
        {
            Path = path,
            Succeeded = writeResult.Succeeded,
            Created = writeResult.Succeeded,
            Error = writeResult.Error
        };
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

    private static string BuildSummary(bool isReady, string profileId, int issueCount, string handoffSummary)
    {
        return isReady
            ? $"Launch plan ready for profile '{profileId}' based on {handoffSummary}."
            : $"Launch plan blocked for profile '{profileId}' with {issueCount} issue(s) based on {handoffSummary}.";
    }
}

public sealed class BrowserClientLaunchPlan
{
    public bool IsReady { get; set; }
    public string ProfileId { get; set; } = "default";
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public bool SettingsPrepared { get; set; }
    public bool StartupProfilePrepared { get; set; }
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public double TotalMs { get; set; }
    public BrowserClientLaunchAsset[] Assets { get; set; } = Array.Empty<BrowserClientLaunchAsset>();
    public BrowserClientLaunchIssue[] Issues { get; set; } = Array.Empty<BrowserClientLaunchIssue>();
    public string Summary { get; set; } = string.Empty;
}

public sealed class BrowserClientLaunchAsset
{
    public string Id { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public bool IsRequired { get; set; }
    public long Length { get; set; }
    public string Summary { get; set; } = string.Empty;
}

public sealed class BrowserClientLaunchIssue
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public sealed class BrowserFilePreparation
{
    public string Path { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    public bool Created { get; set; }
    public string Error { get; set; } = string.Empty;
}
