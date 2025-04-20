using Sustainsys.Saml2.AspNetCore2;

namespace DynAuth.Saml2.Abstraction;

public interface ISamlSchemeManager
{
    void AddSamlScheme(string schemeName, Saml2Options options);
}