using System.Security.Claims;
using DynAuth.Abstraction;
using DynAuth.OpenIdConnect.Abstraction;
using DynAuth.Saml2.Abstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Sustainsys.Saml2;
using Sustainsys.Saml2.AspNetCore2;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.WebSso;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    private readonly IOidcSchemeManager _oidcSchemeManager;
    private readonly ISamlSchemeManager _samlSchemeManager;

    public AuthController(IOidcSchemeManager oidcSchemeManager, ISamlSchemeManager samlSchemeManager)
    {
        _oidcSchemeManager = oidcSchemeManager;
        _samlSchemeManager = samlSchemeManager;
    }

    public IActionResult AddOidcScheme()
    {
        var options = new OpenIdConnectOptions
        {
            SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,
            Authority = "https://accounts.google.com",
            ClientId = "978248021337-4h6k77vcouc9r4dihb9su29540d4m0ki.apps.googleusercontent.com",
            ClientSecret = "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",
            ResponseType = "code",
            SaveTokens = true,
            CallbackPath = $"/signin-google-test",
        };
        
        _oidcSchemeManager.AddScheme("google-test", options);

        return Ok("Scheme added. Now visit /auth/login?scheme=google-test");
    }
    
    public IActionResult AddSamlScheme()
    {
        var options = new Saml2Options
        {
            SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,
            SPOptions =
            {
                EntityId = new EntityId("https://localhost:7119"),
                ModulePath = "/signin-azure-saml2"
            }
        };
        
        var idp = new IdentityProvider(new EntityId("https://sts.windows.net/16e2eac8-c69c-4976-919b-4c3a48c2c0f7/"),
            options.SPOptions)
        {
            Binding = Saml2BindingType.HttpPost,
            LoadMetadata = true,
            MetadataLocation = "https://login.microsoftonline.com/16e2eac8-c69c-4976-919b-4c3a48c2c0f7/federationmetadata/2007-06/federationmetadata.xml?appid=b88b09ee-52b4-4454-8c1f-fee87cf6db58"
        };

        options.IdentityProviders.Add(idp);
        
        _samlSchemeManager.AddScheme("azure", options);

        return Ok("Scheme added. Now visit /auth/login?scheme=azure");
    }

    public IActionResult RemoveSamlScheme()
    {
        _samlSchemeManager.RemoveScheme("azure");
        
        return Ok("Scheme removed. Visit /auth/login?scheme=azure to ensure it is removed.");
    }

    public IActionResult Login(string scheme)
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(Callback))
        };
        return Challenge(props, scheme);
    }
    
    public IActionResult Callback()
    {
        return Json(User.Claims.Select(c => new { c.Type, c.Value }));
    }    
}