using DynAuth.Abstraction;
using DynAuth.Saml2.Abstraction;
using Sustainsys.Saml2.AspNetCore2;

namespace DynAuth.Saml2;

internal class SamlSchemeManager : ISamlSchemeManager
{
    private readonly IDynamicSchemeManager<Saml2Options> _schemeManager;

    public SamlSchemeManager(IDynamicSchemeManager<Saml2Options> schemeManager)
    {
        _schemeManager = schemeManager;
    }
    public void AddScheme(string schemeName, Saml2Options options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrEmpty(schemeName);

        _schemeManager.AddScheme(schemeName, options, typeof(Saml2Handler));
    }

    public void RemoveScheme(string schemeName)
    {
        ArgumentException.ThrowIfNullOrEmpty(schemeName);

        _schemeManager.RemoveScheme(schemeName);
    }
}