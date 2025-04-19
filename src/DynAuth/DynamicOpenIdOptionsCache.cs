using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace DynAuth;

public class DynamicOpenIdOptionsCache : IOptionsMonitorCache<OpenIdConnectOptions>
{
    private readonly ConcurrentDictionary<string, OpenIdConnectOptions> _options = new();

    public bool TryAdd(string? name, OpenIdConnectOptions options)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _options.TryAdd(name, options);
    }

    public bool TryRemove(string? name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _options.TryRemove(name, out _);
    }

    public OpenIdConnectOptions GetOrAdd(string? name, Func<OpenIdConnectOptions> createOptions)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _options.GetOrAdd(name, _ => createOptions());
    }

    public void Clear() => _options.Clear();
}