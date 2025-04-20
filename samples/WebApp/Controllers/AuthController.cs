using System.Security.Claims;
using DynAuth.Abstraction;
using DynAuth.OpenIdConnect.Abstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    private readonly IOidcSchemeManager _oidcSchemeManager;

    public AuthController(IOidcSchemeManager oidcSchemeManager)
    {
        _oidcSchemeManager = oidcSchemeManager;
    }

    public async Task<IActionResult> AddScheme()
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
        
        _oidcSchemeManager.AddOidcScheme("google-test", options);

        return Ok("Scheme added. Now visit /auth/login/google-test");
    }

    public IActionResult Login(string scheme)
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(Callback)),
            Items =
            {
                { "returnUrl", "/" },
                { "scheme", scheme },
            }
        };
        return Challenge(props, scheme);
    }
    
    public async Task<IActionResult> TestSignIn()
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "Test User")
        }, "Cookies");

        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync("Cookies", principal);

        return Ok("Signed in");
    }

    public IActionResult Callback()
    {
        return Json(User.Claims.Select(c => new { c.Type, c.Value }));
    }    
}