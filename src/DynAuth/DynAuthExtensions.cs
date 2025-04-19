using DynAuth.Abstraction;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace DynAuth;

public static class DynAuthExtensions
{
    public static IServiceCollection AddDynAuth(this IServiceCollection services)
    {
        services.AddSingleton<IDynamicOpenIdOptionsRegistry, DynamicOpenIdOptionsRegistry>();
        services.AddSingleton<IOptionsFactory<OpenIdConnectOptions>, DynamicOpenIdOptionsFactory>();
        services.AddSingleton<IOptionsMonitorCache<OpenIdConnectOptions>, DynamicOpenIdOptionsCache>();
        services.AddSingleton<IDynamicSchemeManager, DynamicSchemeManager>();

        return services;
    }
}