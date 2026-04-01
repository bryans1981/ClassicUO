namespace BrowserHost.Services;

public interface IBrowserClientStartupIntegration
{
    ValueTask<BrowserClientStartupRun> PrepareStartupRunAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientStartupIntegrationService : IBrowserClientStartupIntegration
{
    private readonly IBrowserClientEntrypoint _entrypoint;

    public BrowserClientStartupIntegrationService(IBrowserClientEntrypoint entrypoint)
    {
        _entrypoint = entrypoint;
    }

    public async ValueTask<BrowserClientStartupRun> PrepareStartupRunAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientLaunchPlan launchPlan = await _entrypoint.PrepareLaunchAsync(request, profileId);

        BrowserClientStartupStep[] steps =
        [
            CreateStep(
                "bootstrap-assets",
                "Validate bootstrap assets",
                launchPlan.ReadyAssetCount == 3,
                $"{launchPlan.ReadyAssetCount} / 3 required assets ready."
            ),
            CreateStep(
                "config",
                "Prepare config files",
                launchPlan.SettingsPrepared,
                launchPlan.SettingsPrepared ? $"Ready at {launchPlan.SettingsFilePath}" : "Config file preparation failed."
            ),
            CreateStep(
                "profile",
                "Prepare startup profile",
                launchPlan.StartupProfilePrepared,
                launchPlan.StartupProfilePrepared ? $"Ready at {launchPlan.StartupProfilePath}" : "Startup profile preparation failed."
            ),
            CreateStep(
                "launch",
                "Create startup launch contract",
                launchPlan.IsReady,
                launchPlan.Summary
            )
        ];

        int completedSteps = steps.Count(static x => x.Succeeded);

        return new BrowserClientStartupRun
        {
            IsReady = launchPlan.IsReady,
            ProfileId = launchPlan.ProfileId,
            LaunchPlan = launchPlan,
            CompletedSteps = completedSteps,
            TotalSteps = steps.Length,
            Steps = steps,
            TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds,
            Summary = launchPlan.IsReady
                ? $"Startup run prepared with {completedSteps} / {steps.Length} steps complete."
                : $"Startup run blocked with {completedSteps} / {steps.Length} steps complete."
        };
    }

    private static BrowserClientStartupStep CreateStep(string id, string title, bool succeeded, string detail)
    {
        return new BrowserClientStartupStep
        {
            Id = id,
            Title = title,
            Succeeded = succeeded,
            Detail = detail
        };
    }
}

public sealed class BrowserClientStartupRun
{
    public bool IsReady { get; set; }
    public string ProfileId { get; set; } = "default";
    public BrowserClientLaunchPlan LaunchPlan { get; set; } = new();
    public int CompletedSteps { get; set; }
    public int TotalSteps { get; set; }
    public BrowserClientStartupStep[] Steps { get; set; } = Array.Empty<BrowserClientStartupStep>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}

public sealed class BrowserClientStartupStep
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    public string Detail { get; set; } = string.Empty;
}
