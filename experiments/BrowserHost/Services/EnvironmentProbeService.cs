using Microsoft.JSInterop;

namespace BrowserHost.Services;

public sealed class EnvironmentProbeService
{
    private readonly IJSRuntime _js;

    public EnvironmentProbeService(IJSRuntime js)
    {
        _js = js;
    }

    public bool IsBrowserRuntime => OperatingSystem.IsBrowser();

    public async Task<BrowserProbeResult> ProbeAsync()
    {
        var details = await _js.InvokeAsync<BrowserProbeResult>("classicuoProbe.getEnvironment");
        details.IsBrowserRuntime = IsBrowserRuntime;
        return details;
    }
}

public sealed class BrowserProbeResult
{
    public bool IsBrowserRuntime { get; set; }
    public bool HasWindow { get; set; }
    public bool HasIndexedDb { get; set; }
    public bool HasFileSystemAccessApi { get; set; }
    public bool HasOpfsApi { get; set; }
    public bool HasSharedArrayBuffer { get; set; }
    public string UserAgent { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
}
