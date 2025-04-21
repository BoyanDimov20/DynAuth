
namespace DynAuth.Abstraction;

public interface IDynamicSchemeManager<TOptions> where TOptions : class
{
    void AddScheme(string schemeName, TOptions options, Type handler);
    void RemoveScheme(string schemeName);
}