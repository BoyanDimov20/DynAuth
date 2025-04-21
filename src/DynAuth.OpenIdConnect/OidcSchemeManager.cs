using DynAuth.Abstraction;
using DynAuth.OpenIdConnect.Abstraction;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace DynAuth.OpenIdConnect;

internal class OidcSchemeManager : IOidcSchemeManager
{
    private readonly IDynamicSchemeManager<OpenIdConnectOptions> _schemeManager;
    private readonly IOptionsFactory<OpenIdConnectOptions> _optionsFactory;

    public OidcSchemeManager(IDynamicSchemeManager<OpenIdConnectOptions> schemeManager,
        IOptionsFactory<OpenIdConnectOptions> optionsFactory)
    {
        _schemeManager = schemeManager;
        _optionsFactory = optionsFactory;
    }


    public void AddScheme(string schemeName, OpenIdConnectOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrEmpty(schemeName);

        _schemeManager.AddScheme(schemeName, options, typeof(OpenIdConnectHandler));
    }

    public void RemoveScheme(string schemeName)
    {
        ArgumentException.ThrowIfNullOrEmpty(schemeName);

        _schemeManager.RemoveScheme(schemeName);
    }
}