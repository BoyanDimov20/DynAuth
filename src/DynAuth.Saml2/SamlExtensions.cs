using DynAuth.Saml2.Abstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Sustainsys.Saml2.AspNetCore2;

namespace DynAuth.Saml2;

public static class SamlExtensions
{
    public static AuthenticationBuilder AddDynAuthSaml(this AuthenticationBuilder builder)
    {
        builder.Services.AddDynAuth<Saml2Options>()
            .AddTransient<ISamlSchemeManager, SamlSchemeManager>();
        
        builder.AddSaml2(options => { });
        
        return builder;
    }
}