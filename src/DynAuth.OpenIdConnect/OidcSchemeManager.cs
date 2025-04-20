using DynAuth.Abstraction;
using DynAuth.OpenIdConnect.Abstraction;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace DynAuth.OpenIdConnect;

internal class OidcSchemeManager : IOidcSchemeManager
{
    private readonly IDynamicSchemeManager<OpenIdConnectOptions> _schemeManager;
    private readonly IOptionsFactory<OpenIdConnectOptions> _optionsFactory;
    private readonly IDynamicOpenIdOptionsRegistry<OpenIdConnectOptions> _optionsRegistry;

    public OidcSchemeManager(IDynamicSchemeManager<OpenIdConnectOptions> schemeManager,
        IOptionsFactory<OpenIdConnectOptions> optionsFactory,
        IDynamicOpenIdOptionsRegistry<OpenIdConnectOptions> optionsRegistry)
    {
        _schemeManager = schemeManager;
        _optionsFactory = optionsFactory;
        _optionsRegistry = optionsRegistry;
    }


    public void AddOidcScheme(string schemeName, OpenIdConnectOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrEmpty(schemeName);

        _optionsRegistry.RegisterBaseOptions(schemeName, options);
        var finalOptions = _optionsFactory.Create(schemeName);
        finalOptions.Validate();

        _schemeManager.AddScheme(schemeName, finalOptions, typeof(OpenIdConnectHandler));
    }
}