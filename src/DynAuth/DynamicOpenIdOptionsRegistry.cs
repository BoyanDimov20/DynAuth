using DynAuth.Abstraction;
using Microsoft.AspNetCore.Authentication;

namespace DynAuth;

internal class DynamicOpenIdOptionsRegistry<TOptions> : IDynamicOpenIdOptionsRegistry<TOptions> where TOptions : AuthenticationSchemeOptions
{
    private readonly Dictionary<string, TOptions> _registry = new(StringComparer.OrdinalIgnoreCase);

    public void RegisterBaseOptions(string scheme, TOptions options)
        => _registry[scheme] = options;

    public bool TryGet(string scheme, out TOptions options)
        => _registry.TryGetValue(scheme, out options);
}
