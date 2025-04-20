using DynAuth.Abstraction;
using Microsoft.Extensions.Options;

namespace DynAuth;

public class DynamicOpenIdOptionsFactory<TOptions> : IOptionsFactory<TOptions> where TOptions : class, new()
{
    private readonly IEnumerable<IConfigureOptions<TOptions>> _configurators;
    private readonly IEnumerable<IPostConfigureOptions<TOptions>> _postConfigurators;
    private readonly IDynamicOpenIdOptionsRegistry<TOptions> _registry;

    public DynamicOpenIdOptionsFactory(
        IEnumerable<IConfigureOptions<TOptions>> configurators,
        IEnumerable<IPostConfigureOptions<TOptions>> postConfigurators,
        IDynamicOpenIdOptionsRegistry<TOptions> registry)
    {
        _configurators = configurators;
        _postConfigurators = postConfigurators;
        _registry = registry;
    }

    public TOptions Create(string name)
    {
        var options = _registry.TryGet(name, out var baseOptions)
            ? baseOptions
            : new TOptions();

        foreach (var config in _configurators)
        {
            if (config is IConfigureNamedOptions<TOptions> named)
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