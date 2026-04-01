namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBootstrapLoop
{
    ValueTask<BrowserClientRuntimeBootstrapLoopResult> RunAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBootstrapLoopService : IBrowserClientRuntimeBootstrapLoop
{
    private readonly IBrowserClientStartupSessionRunner _startupSessionRunner;

    public BrowserClientRuntimeBootstrapLoopService(IBrowserClientStartupSessionRunner startupSessionRunner)
    {
        _startupSessionRunner = startupSessionRunner;
    }

    public async ValueTask<BrowserClientRuntimeBootstrapLoopResult> RunAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientStartupSessionRunnerResult runner = await _startupSessionRunner.RunAsync(profileId);

        BrowserClientRuntimeBootstrapLoopResult result = new()
        {
            ProfileId = runner.ProfileId,
            SessionId = runner.SessionId,
            SessionPath = runner.SessionPath,
            RunnerVersion = runner.RunnerVersion,
            ExecutionVersion = runner.ExecutionVersion,
            HandshakeVersion = runner.HandshakeVersion,
            PacketVersion = runner.PacketVersion,
            ContractVersion = runner.ContractVersion,
            Exists = runner.Exists,
            ReadSucceeded = runner.ReadSucceeded
        };

        if (!runner.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime bootstrap loop blocked for profile '{runner.ProfileId}'.";
            result.Error = runner.Error;
            return result;
        }

        result.IsReady = true;
        result.LoopVersion = "runtime-bootstrap-loop-v1";
        result.LaunchMode = runner.LaunchMode;
        result.AssetRootPath = runner.AssetRootPath;
        result.ProfilesRootPath = runner.ProfilesRootPath;
        result.CacheRootPath = runner.CacheRootPath;
        result.ConfigRootPath = runner.ConfigRootPath;
        result.SettingsFilePath = runner.SettingsFilePath;
        result.StartupProfilePath = runner.StartupProfilePath;
        result.RequiredAssets = runner.RequiredAssets;
        result.ReadyAssetCount = runner.ReadyAssetCount;
        result.CompletedSteps = runner.CompletedSteps;
        result.TotalSteps = runner.TotalSteps;
        result.Phases =
        [
            new BrowserClientRuntimeBootstrapPhase
            {
                Id = "bootstrap",
                Title = "Bootstrap assets",
                Succeeded = true,
                Detail = $"{runner.ReadyAssetCount} / {runner.RequiredAssets.Length} required assets ready."
            },
            new BrowserClientRuntimeBootstrapPhase
            {
                Id = "profile",
                Title = "Profile and config",
                Succeeded = true,
                Detail = $"Using {runner.SettingsFilePath} and {runner.StartupProfilePath}."
            },
            new BrowserClientRuntimeBootstrapPhase
            {
                Id = "runtime",
                Title = "Runtime pass",
                Succeeded = true,
                Detail = runner.RunSummary
            }
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime bootstrap loop ready for profile '{runner.ProfileId}' with {result.Phases.Length} phase(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBootstrapLoopResult
{
    public bool IsReady { get; set; }
    public string LoopVersion { get; set; } = string.Empty;
    public string RunnerVersion { get; set; } = string.Empty;
    public string ExecutionVersion { get; set; } = string.Empty;
    public string HandshakeVersion { get; set; } = string.Empty;
    public string PacketVersion { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ProfileId { get; set; } = "default";
    public string SessionId { get; set; } = string.Empty;
    public string SessionPath { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public string AssetRootPath { get; set; } = string.Empty;
    public string ProfilesRootPath { get; set; } = string.Empty;
    public string CacheRootPath { get; set; } = string.Empty;
    public string ConfigRootPath { get; set; } = string.Empty;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public int ReadyAssetCount { get; set; }
    public int CompletedSteps { get; set; }
    public int TotalSteps { get; set; }
    public BrowserClientRuntimeBootstrapPhase[] Phases { get; set; } = Array.Empty<BrowserClientRuntimeBootstrapPhase>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserClientRuntimeBootstrapPhase
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    public string Detail { get; set; } = string.Empty;
}
