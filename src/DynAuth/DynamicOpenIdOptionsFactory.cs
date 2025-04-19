using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace DynAuth;

public class DynamicOpenIdOptionsFactory : IOptionsFactory<OpenIdConnectOptions>
{
    private readonly IEnumerable<IConfigureOptions<OpenIdConnectOptions>> _configurators;
    private readonly IEnumerable<IPostConfigureOptions<OpenIdConnectOptions>> _postConfigurators;
    private readonly DynamicOpenIdOptionsRegistry _registry;

    public DynamicOpenIdOptionsFactory(
        IEnumerable<IConfigureOptions<OpenIdConnectOptions>> configurators,
        IEnumerable<IPostConfigureOptions<OpenIdConnectOptions>> postConfigurators,
        DynamicOpenIdOptionsRegistry registry)
    {
        _configurators = configurators;
        _postConfigurators = postConfigurators;
        _registry = registry;
    }

    public OpenIdConnectOptions Create(string name)
    {
        var options = _registry.TryGet(name, out var baseOptions)
            ? baseOptions
            : new OpenIdConnectOptions();

        foreach (var config in _configurators)
        {
            if (config is IConfigureNamedOptions<OpenIdConnectOptions> named)
                named.Configure(name, options);
            else
                config.Configure(options);
        }

        foreach (var post in _postConfigurators)
        {
            post.PostConfigure(name, options);
        }

        return options;
    }

}