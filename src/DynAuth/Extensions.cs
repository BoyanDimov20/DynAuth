using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace DynAuth;

public static class Extensions
{
    public static IServiceCollection AddDynAuth(this IServiceCollection services)
    {
        services.AddSingleton<DynamicOpenIdOptionsRegistry>();
        //services.AddSingleton<IConfigureNamedOptions<OpenIdConnectOptions>, DynamicOpenIdPostConfigurator>();
        services.AddSingleton<IOptionsFactory<OpenIdConnectOptions>, DynamicOpenIdOptionsFactory>();
        services.AddSingleton<IOptionsMonitorCache<OpenIdConnectOptions>, DynamicOpenIdOptionsCache>();
        services.AddSingleton<DynamicSchemeManager>();

        return services;
    }
}