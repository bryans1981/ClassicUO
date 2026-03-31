namespace BrowserHost.Services;

public sealed class BrowserProcessedAssetCacheService
{
    private readonly Dictionary<string, object> _entries = new(StringComparer.OrdinalIgnoreCase);

    public ValueTask<T> GetOrAddAsync<T>(string key, Func<ValueTask<T>> factory)
    {
        if (_entries.TryGetValue(key, out object? value) && value is T typed)
        {
            return ValueTask.FromResult(typed);
        }

        return CreateAsync(key, factory);
    }

    public bool Contains(string key)
    {
        return _entries.ContainsKey(key);
    }

    public BrowserProcessedAssetCacheSummary GetSummary()
    {
        return new BrowserProcessedAssetCacheSummary
        {
            EntryCount = _entries.Count,
            Keys = _entries.Keys.OrderBy(static x => x, StringComparer.OrdinalIgnoreCase).ToArray()
        };
    }

    public void Clear()
    {
        _entries.Clear();
    }

    private async ValueTask<T> CreateAsync<T>(string key, Func<ValueTask<T>> factory)
    {
        T value = await factory();
        _entries[key] = value!;
        return value;
    }
}

public sealed class BrowserProcessedAssetCacheSummary
{
    public int EntryCount { get; set; }
    public string[] Keys { get; set; } = Array.Empty<string>();
}
