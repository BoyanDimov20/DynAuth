using DynAuth.Abstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DynAuth;

internal class DynamicSchemeManager<TOptions> : IDynamicSchemeManager<TOptions> where TOptions : AuthenticationSchemeOptions
{
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IOptionsMonitorCache<TOptions> _optionsCache;
    private readonly IDynamicOpenIdOptionsRegistry<TOptions> _optionsRegistry;
    private readonly IOptionsFactory<TOptions> _optionsFactory;

    public DynamicSchemeManager(
        IAuthenticationSchemeProvider schemeProvider,
        IOptionsMonitorCache<TOptions> optionsCache, 
        IDynamicOpenIdOptionsRegistry<TOptions> optionsRegistry,
        IOptionsFactory<TOptions> optionsFactory)
    {
        _schemeProvider = schemeProvider;
        _optionsCache = optionsCache;
        _optionsRegistry = optionsRegistry;
        _optionsFactory = optionsFactory;
    }

    public void AddScheme(string schemeName, TOptions options, Type handler)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrEmpty(schemeName);
        
        _optionsRegistry.RegisterBaseOptions(schemeName, options);
        var finalOptions = _optionsFactory.Create(schemeName);
        finalOptions.Validate();
        
        _optionsCache.TryAdd(schemeName, finalOptions);

        // Register the scheme
        var scheme = new AuthenticationScheme(schemeName, schemeName, handler);
        _schemeProvider.AddScheme(scheme);

    }

    
}