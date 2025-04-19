using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace DynAuth.Abstraction;

public interface IDynamicSchemeManager
{
    Task AddOpenIdSchemeAsync(string schemeName, OpenIdConnectOptions options);
}