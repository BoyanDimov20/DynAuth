using DynAuth.Abstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DynAuth;

public static class DynAuthExtensions
{
    public static IServiceCollection AddDynAuth<TOptions>(this IServiceCollection services) where TOptions : AuthenticationSchemeOptions, new()
    {
        services.AddSingleton<IDynamicOpenIdOptionsRegistry<TOptions>, DynamicOpenIdOptionsRegistry<TOptions>>();
        services.AddSingleton<IOptionsFactory<TOptions>, DynamicOpenIdOptionsFactory<TOptions>>();
        services.AddSingleton<IOptionsMonitorCache<TOptions>, DynamicOpenIdOptionsCache<TOptions>>();
        services.AddSingleton<IDynamicSchemeManager<TOptions>, DynamicSchemeManager<TOptions>>();
        
        return services;
    }
}