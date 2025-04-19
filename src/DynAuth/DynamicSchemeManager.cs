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
    private readonly DynamicOpenIdOptionsRegistry _optionsRegistry;

    public DynamicSchemeManager(
        IAuthenticationSchemeProvider schemeProvider,
        IOptionsMonitorCache<OpenIdConnectOptions> optionsCache,
        IOptionsFactory<OpenIdConnectOptions> optionsFactory,
        DynamicOpenIdOptionsRegistry optionsRegistry)
    {
        _schemeProvider = schemeProvider;
        _optionsCache = optionsCache;
        _optionsFactory = optionsFactory;
        _optionsRegistry = optionsRegistry;
    }

    public Task AddOpenIdSchemeAsync(string schemeName, string authority, string clientId, string clientSecret)
    {
        var handlerType = typeof(OpenIdConnectHandler);

        // Add to options manually
        var options = new OpenIdConnectOptions
        {
            SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,
            Authority = authority,
            ClientId = clientId,
            ClientSecret = clientSecret,
            ResponseType = "code",
            SaveTokens = true,
            CallbackPath = $"/signin-{schemeName}",
        };
        _optionsRegistry.RegisterBaseOptions(schemeName, options);
        
        var finalOptions = _optionsFactory.Create(schemeName);

        // Register in the options monitor cache (advanced: requires custom implementation if needed)
        _optionsCache.TryAdd(schemeName, finalOptions);
        

        // Register the scheme
        var scheme = new AuthenticationScheme(schemeName, schemeName, handlerType);
        _schemeProvider.AddScheme(scheme);

        return Task.CompletedTask;
    }


    public Task AddOpenIdSchemeAsync(string schemeName, OpenIdConnectOptions options)
    {
        _optionsRegistry.RegisterBaseOptions(schemeName, options);
        
        var finalOptions = _optionsFactory.Create(schemeName);

        // Register in the options monitor cache (advanced: requires custom implementation if needed)
        _optionsCache.TryAdd(schemeName, finalOptions);
        

        // Register the scheme
        var scheme = new AuthenticationScheme(schemeName, schemeName, typeof(OpenIdConnectHandler));
        _schemeProvider.AddScheme(scheme);

        return Task.CompletedTask;
    }
}