using DynAuth.Abstraction;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace DynAuth;

public class DynamicOpenIdOptionsRegistry : IDynamicOpenIdOptionsRegistry
{
    private readonly Dictionary<string, OpenIdConnectOptions> _registry = new(StringComparer.OrdinalIgnoreCase);

    public void RegisterBaseOptions(string scheme, OpenIdConnectOptions options)
        => _registry[scheme] = options;

    public bool TryGet(string scheme, out OpenIdConnectOptions options)
        => _registry.TryGetValue(scheme, out options);
}
