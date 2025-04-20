using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace DynAuth.OpenIdConnect.Abstraction;

public interface IOidcSchemeManager
{
    void AddOidcScheme(string schemeName, OpenIdConnectOptions options);
}