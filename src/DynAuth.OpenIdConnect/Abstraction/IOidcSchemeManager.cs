using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace DynAuth.OpenIdConnect.Abstraction;

public interface IOidcSchemeManager
{
    void AddScheme(string schemeName, OpenIdConnectOptions options);
    void RemoveScheme(string schemeName);
}