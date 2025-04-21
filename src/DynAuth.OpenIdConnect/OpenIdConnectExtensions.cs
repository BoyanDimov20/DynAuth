using DynAuth.OpenIdConnect.Abstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace DynAuth.OpenIdConnect;

public static class OpenIdConnectExtensions
{
    public static AuthenticationBuilder AddDynAuthOpenIdConnect(this AuthenticationBuilder builder)
    {
        builder.Services.AddDynAuth<OpenIdConnectOptions>()
            .AddTransient<IOidcSchemeManager, OidcSchemeManager>();

        builder.AddOpenIdConnect();
        return builder;
    }
}