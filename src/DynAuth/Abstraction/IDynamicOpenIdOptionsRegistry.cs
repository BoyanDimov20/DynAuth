namespace DynAuth.Abstraction;

public interface IDynamicOpenIdOptionsRegistry<TOptions> where TOptions : class
{
    public void RegisterBaseOptions(string scheme, TOptions options);
    public bool TryGet(string scheme, out TOptions options);
}