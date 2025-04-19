using System.Text;
using DynAuth.Abstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace DynAuth;

public class DynamicSchemeManager : IDynamicSchemeManager
{
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IOptionsMonitorCache<OpenIdConnectOptions> _optionsCache;
    private readonly IOptionsFactory<OpenIdConnectOptions> _optionsFactory;
    private readonly IDynamicOpenIdOptionsRegistry _optionsRegistry;

    public DynamicSchemeManager(
        IAuthenticationSchemeProvider schemeProvider,
        IOptionsMonitorCache<OpenIdConnectOptions> optionsCache,
        IOptionsFactory<OpenIdConnectOptions> optionsFactory,
        IDynamicOpenIdOptionsRegistry optionsRegistry)
    {
        _schemeProvider = schemeProvider;
        _optionsCache = optionsCache;
        _optionsFactory = optionsFactory;
        _optionsRegistry = optionsRegistry;
    }

    public Task AddOpenIdSchemeAsync(string schemeName, OpenIdConnectOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrEmpty(schemeName);
        
        options.Validate();

        _optionsRegistry.RegisterBaseOptions(schemeName, options);
        var finalOptions = _optionsFactory.Create(schemeName);
        _optionsCache.TryAdd(schemeName, finalOptions);
        

        // Register the scheme
        var scheme = new AuthenticationScheme(schemeName, schemeName, typeof(OpenIdConnectHandler));
        _schemeProvider.AddScheme(scheme);

        return Task.CompletedTask;
    }
}