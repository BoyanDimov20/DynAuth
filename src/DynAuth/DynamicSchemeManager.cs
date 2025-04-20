using DynAuth.Abstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DynAuth;

internal class DynamicSchemeManager<TOptions> : IDynamicSchemeManager<TOptions> where TOptions : class, new()
{
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IOptionsMonitorCache<TOptions> _optionsCache;

    public DynamicSchemeManager(
        IAuthenticationSchemeProvider schemeProvider,
        IOptionsMonitorCache<TOptions> optionsCache)
    {
        _schemeProvider = schemeProvider;
        _optionsCache = optionsCache;
    }

    public void AddScheme(string schemeName, TOptions options, Type handler)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrEmpty(schemeName);
        
        _optionsCache.TryAdd(schemeName, options);

        // Register the scheme
        var scheme = new AuthenticationScheme(schemeName, schemeName, handler);
        _schemeProvider.AddScheme(scheme);

    }

    
}