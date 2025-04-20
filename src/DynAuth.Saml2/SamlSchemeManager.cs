using DynAuth.Abstraction;
using DynAuth.Saml2.Abstraction;
using Microsoft.Extensions.Options;
using Sustainsys.Saml2.AspNetCore2;

namespace DynAuth.Saml2;

internal class SamlSchemeManager : ISamlSchemeManager
{
    private readonly IDynamicSchemeManager<Saml2Options> _schemeManager;
    private readonly IOptionsFactory<Saml2Options> _optionsFactory;
    private readonly IDynamicOpenIdOptionsRegistry<Saml2Options> _optionsRegistry;

    public SamlSchemeManager(IDynamicSchemeManager<Saml2Options> schemeManager,
        IOptionsFactory<Saml2Options> optionsFactory,
        IDynamicOpenIdOptionsRegistry<Saml2Options> optionsRegistry)
    {
        _schemeManager = schemeManager;
        _optionsFactory = optionsFactory;
        _optionsRegistry = optionsRegistry;
    }
    public void AddSamlScheme(string schemeName, Saml2Options options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrEmpty(schemeName);

        _schemeManager.AddScheme(schemeName, options, typeof(Saml2Handler));
    }
}