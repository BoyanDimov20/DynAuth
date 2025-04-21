using Sustainsys.Saml2.AspNetCore2;

namespace DynAuth.Saml2.Abstraction;

public interface ISamlSchemeManager
{
    void AddScheme(string schemeName, Saml2Options options);
    void RemoveScheme(string schemeName);
}