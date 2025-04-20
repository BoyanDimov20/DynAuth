using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace DynAuth;

public class DynamicOpenIdOptionsCache<TOptions> : IOptionsMonitorCache<TOptions> where TOptions : class
{
    private readonly ConcurrentDictionary<string, TOptions> _options = new();

    public bool TryAdd(string? name, TOptions options)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _options.TryAdd(name, options);
    }

    public bool TryRemove(string? name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _options.TryRemove(name, out _);
    }

    public TOptions GetOrAdd(string? name, Func<TOptions> createOptions)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _options.GetOrAdd(name, _ => createOptions());
    }

    public void Clear() => _options.Clear();
}