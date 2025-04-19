using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace DynAuth.Abstraction;

public interface IDynamicOpenIdOptionsRegistry
{
    public void RegisterBaseOptions(string scheme, OpenIdConnectOptions options);
    public bool TryGet(string scheme, out OpenIdConnectOptions options);
}