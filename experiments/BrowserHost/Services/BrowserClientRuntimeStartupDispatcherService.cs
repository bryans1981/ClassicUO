namespace BrowserHost.Services;

public interface IBrowserClientRuntimeStartupDispatcher
{
    ValueTask<BrowserClientRuntimeStartupDispatcherResult> DispatchAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeStartupDispatcherService : IBrowserClientRuntimeStartupDispatcher
{
    private readonly IBrowserClientRuntimeStartupTransitionDriver _runtimeStartupTransitionDriver;

    public BrowserClientRuntimeStartupDispatcherService(IBrowserClientRuntimeStartupTransitionDriver runtimeStartupTransitionDriver)
    {
        _runtimeStartupTransitionDriver = runtimeStartupTransitionDriver;
    }

    public async ValueTask<BrowserClientRuntimeStartupDispatcherResult> DispatchAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeStartupTransitionDriverResult driver = await _runtimeStartupTransitionDriver.DriveAsync(profileId);

        BrowserClientRuntimeStartupDispatcherResult result = new()
        {
            ProfileId = driver.ProfileId,
            SessionId = driver.SessionId,
            SessionPath = driver.SessionPath,
            DriverVersion = driver.DriverVersion,
            StateMachineVersion = driver.StateMachineVersion,
            StateVersion = driver.StateVersion,
            CycleVersion = driver.CycleVersion,
            InvocationVersion = driver.InvocationVersion,
            LoopVersion = driver.LoopVersion,
            RunnerVersion = driver.RunnerVersion,
            ExecutionVersion = driver.ExecutionVersion,
            HandshakeVersion = driver.HandshakeVersion,
            PacketVersion = driver.PacketVersion,
            ContractVersion = driver.ContractVersion,
            Exists = driver.Exists,
            ReadSucceeded = driver.ReadSucceeded
        };

        if (!driver.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime startup dispatcher blocked for profile '{driver.ProfileId}'.";
            result.Error = driver.Error;
            return result;
        }

        result.IsReady = true;
        result.DispatcherVersion = "runtime-startup-dispatcher-v1";
        result.LaunchMode = driver.LaunchMode;
        result.AssetRootPath = driver.AssetRootPath;
        result.ProfilesRootPath = driver.ProfilesRootPath;
        result.CacheRootPath = driver.CacheRootPath;
        result.ConfigRootPath = driver.ConfigRootPath;
        result.SettingsFilePath = driver.SettingsFilePath;
        result.StartupProfilePath = driver.StartupProfilePath;
        result.RequiredAssets = driver.RequiredAssets;
        result.ReadyAssetCount = driver.ReadyAssetCount;
        result.CompletedSteps = driver.CompletedSteps;
        result.TotalSteps = driver.TotalSteps;
        result.Phases = driver.Phases;
        result.CurrentState = driver.CurrentState;
        result.States = driver.States;
        result.Transitions = driver.Transitions;
        result.DispatchTargets =
        [
            "asset-bootstrap",
            "profile-bootstrap",
            "runtime-bootstrap"
        ];
        result.DispatchSummary = $"Runtime startup dispatcher prepared {result.DispatchTargets.Length} dispatch target(s) for profile '{driver.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime startup dispatcher ready for profile '{driver.ProfileId}' with {result.DispatchTargets.Length} dispatch target(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeStartupDispatcherResult
{
    public bool IsReady { get; set; }
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
    public string CurrentState { get; set; } = string.Empty;
    public string[] States { get; set; } = Array.Empty<string>();
    public string[] Transitions { get; set; } = Array.Empty<string>();
    public string[] DispatchTargets { get; set; } = Array.Empty<string>();
    public string DispatchSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
