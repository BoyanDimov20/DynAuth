using DynAuth.OpenIdConnect.Abstraction;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace DynAuth.OpenIdConnect;

public static class OpenIdConnectExtensions
{
    public static void AddDynAuthOpenIdConnect(this IServiceCollection services)
    {
        services.AddDynAuth<OpenIdConnectOptions>()
            .AddTransient<IOidcSchemeManager, OidcSchemeManager>();
        
    }
}