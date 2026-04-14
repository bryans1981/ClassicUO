using System.Text.Json;
using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientBootstrapPackage
{
    ValueTask<BrowserClientBootstrapPackageResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientBootstrapPackageService : IBrowserClientBootstrapPackage
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false,
        PropertyNameCaseInsensitive = true
    };

    private readonly IBrowserClientRuntimeBootstrapSession _runtimeBootstrapSession;
    private readonly BrowserStorageService _storageService;

    public BrowserClientBootstrapPackageService(
        IBrowserClientRuntimeBootstrapSession runtimeBootstrapSession,
        BrowserStorageService storageService
    )
    {
        _runtimeBootstrapSession = runtimeBootstrapSession;
        _storageService = storageService;
    }

    public async ValueTask<BrowserClientBootstrapPackageResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBootstrapSessionResult runtimeSession = await _runtimeBootstrapSession.CreateAsync(profileId);
        string normalizedProfileId = NormalizeProfileId(runtimeSession.ProfileId);
        string artifactPath = BrowserVirtualPaths.CacheFile($"startup/{normalizedProfileId}/browser-bootstrap-package.json");

        BrowserClientBootstrapPackageResult result = new()
        {
            ProfileId = normalizedProfileId,
            ArtifactPath = artifactPath,
            RuntimeSessionVersion = runtimeSession.RuntimeSessionVersion,
            ConsumerVersion = runtimeSession.ConsumerVersion,
            HandoffVersion = runtimeSession.HandoffVersion,
            BootSessionVersion = runtimeSession.BootSessionVersion,
            BootFlowVersion = runtimeSession.BootFlowVersion,
            OrchestratorVersion = runtimeSession.OrchestratorVersion,
            CoordinatorVersion = runtimeSession.CoordinatorVersion,
            ControllerVersion = runtimeSession.ControllerVersion,
            DispatcherVersion = runtimeSession.DispatcherVersion,
            DriverVersion = runtimeSession.DriverVersion,
            StateMachineVersion = runtimeSession.StateMachineVersion,
            StateVersion = runtimeSession.StateVersion,
            CycleVersion = runtimeSession.CycleVersion,
            InvocationVersion = runtimeSession.InvocationVersion,
            LoopVersion = runtimeSession.LoopVersion,
            RunnerVersion = runtimeSession.RunnerVersion,
            ExecutionVersion = runtimeSession.ExecutionVersion,
            HandshakeVersion = runtimeSession.HandshakeVersion,
            PacketVersion = runtimeSession.PacketVersion,
            ContractVersion = runtimeSession.ContractVersion,
            Exists = runtimeSession.Exists,
            ReadSucceeded = runtimeSession.ReadSucceeded
        };

        if (!runtimeSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Browser bootstrap package blocked for profile '{normalizedProfileId}'.";
            result.Error = runtimeSession.Error;
            return result;
        }

        result.IsReady = true;
        result.PackageVersion = "browser-bootstrap-package-v1";
        result.LaunchMode = runtimeSession.LaunchMode;
        result.AssetRootPath = runtimeSession.AssetRootPath;
        result.ProfilesRootPath = runtimeSession.ProfilesRootPath;
        result.CacheRootPath = runtimeSession.CacheRootPath;
        result.ConfigRootPath = runtimeSession.ConfigRootPath;
        result.SettingsFilePath = runtimeSession.SettingsFilePath;
        result.StartupProfilePath = runtimeSession.StartupProfilePath;
        result.RequiredAssets = runtimeSession.RequiredAssets;
        result.ReadyAssetCount = runtimeSession.ReadyAssetCount;
        result.CompletedSteps = runtimeSession.CompletedSteps;
        result.TotalSteps = runtimeSession.TotalSteps;
        result.BootFlowStages = runtimeSession.BootFlowStages;
        result.BootSessionStages = runtimeSession.BootSessionStages;
        result.HandoffArtifacts = runtimeSession.HandoffArtifacts;
        result.ConsumedArtifacts = runtimeSession.ConsumedArtifacts;
        result.RuntimeSessionStages = runtimeSession.RuntimeSessionStages;
        result.SourceSummary = runtimeSession.Summary;

        BrowserClientBootstrapPackage package = new()
        {
            PackageVersion = result.PackageVersion,
            ProfileId = result.ProfileId,
            LaunchMode = result.LaunchMode,
            ArtifactPath = result.ArtifactPath,
            AssetRootPath = result.AssetRootPath,
            ProfilesRootPath = result.ProfilesRootPath,
            CacheRootPath = result.CacheRootPath,
            ConfigRootPath = result.ConfigRootPath,
            SettingsFilePath = result.SettingsFilePath,
            StartupProfilePath = result.StartupProfilePath,
            RequiredAssets = result.RequiredAssets,
            ReadyAssetCount = result.ReadyAssetCount,
            CompletedSteps = result.CompletedSteps,
            TotalSteps = result.TotalSteps,
            HandoffArtifacts = result.HandoffArtifacts,
            ConsumedArtifacts = result.ConsumedArtifacts,
            RuntimeSessionStages = result.RuntimeSessionStages,
            SourceSummary = result.SourceSummary,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        string json = JsonSerializer.Serialize(package, JsonOptions);
        BrowserFileWriteResult writeResult = await _storageService.WriteTextAsync(artifactPath, json);
        result.WriteSucceeded = writeResult.Succeeded;

        if (!writeResult.Succeeded)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Browser bootstrap package write failed for profile '{normalizedProfileId}'.";
            result.Error = writeResult.Error;
            return result;
        }

        BrowserFileTextResult readResult = await _storageService.ReadTextAsync(artifactPath);
        result.Exists = readResult.Exists;
        result.ReadSucceeded = readResult.Exists && !string.IsNullOrWhiteSpace(readResult.Text);
        result.Length = readResult.Text.Length;

        if (result.ReadSucceeded)
        {
            BrowserClientBootstrapPackage? readPackage = JsonSerializer.Deserialize<BrowserClientBootstrapPackage>(readResult.Text, JsonOptions);
            result.IsReady = readPackage is not null && string.Equals(readPackage.PackageVersion, result.PackageVersion, StringComparison.Ordinal);
            result.Package = readPackage ?? package;
        }

        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser bootstrap package ready for profile '{normalizedProfileId}'."
            : $"Browser bootstrap package saved but not validated for profile '{normalizedProfileId}'.";

        return result;
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

public sealed class BrowserClientBootstrapPackageResult
{
    public bool IsReady { get; set; }
    public string PackageVersion { get; set; } = string.Empty;
    public string ProfileId { get; set; } = "default";
    public string ArtifactPath { get; set; } = string.Empty;
    public string RuntimeSessionVersion { get; set; } = string.Empty;
    public string ConsumerVersion { get; set; } = string.Empty;
    public string HandoffVersion { get; set; } = string.Empty;
    public string BootSessionVersion { get; set; } = string.Empty;
    public string BootFlowVersion { get; set; } = string.Empty;
    public string OrchestratorVersion { get; set; } = string.Empty;
    public string CoordinatorVersion { get; set; } = string.Empty;
    public string ControllerVersion { get; set; } = string.Empty;
    public string DispatcherVersion { get; set; } = string.Empty;
    public string DriverVersion { get; set; } = string.Empty;
    public string StateMachineVersion { get; set; } = string.Empty;
    public string StateVersion { get; set; } = string.Empty;
    public string CycleVersion { get; set; } = string.Empty;
    public string InvocationVersion { get; set; } = string.Empty;
    public string LoopVersion { get; set; } = string.Empty;
    public string RunnerVersion { get; set; } = string.Empty;
    public string ExecutionVersion { get; set; } = string.Empty;
    public string HandshakeVersion { get; set; } = string.Empty;
    public string PacketVersion { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
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
    public string[] BootFlowStages { get; set; } = Array.Empty<string>();
    public string[] BootSessionStages { get; set; } = Array.Empty<string>();
    public string[] HandoffArtifacts { get; set; } = Array.Empty<string>();
    public string[] ConsumedArtifacts { get; set; } = Array.Empty<string>();
    public string[] RuntimeSessionStages { get; set; } = Array.Empty<string>();
    public string SourceSummary { get; set; } = string.Empty;
    public long Length { get; set; }
    public bool WriteSucceeded { get; set; }
    public double TotalMs { get; set; }
    public BrowserClientBootstrapPackage? Package { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public string? PackageJson { get; set; }
}

public sealed class BrowserClientBootstrapPackage
{
    public string PackageVersion { get; set; } = string.Empty;
    public string ProfileId { get; set; } = "default";
    public string LaunchMode { get; set; } = string.Empty;
    public string ArtifactPath { get; set; } = string.Empty;
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
    public string[] HandoffArtifacts { get; set; } = Array.Empty<string>();
    public string[] ConsumedArtifacts { get; set; } = Array.Empty<string>();
    public string[] RuntimeSessionStages { get; set; } = Array.Empty<string>();
    public string SourceSummary { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; }
}
