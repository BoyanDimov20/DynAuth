using System.Security.Claims;
using DynAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    private readonly DynamicSchemeManager _schemeManager;

    public AuthController(DynamicSchemeManager schemeManager)
    {
        _schemeManager = schemeManager;
    }

    public async Task<IActionResult> AddScheme()
    {
        await _schemeManager.AddOpenIdSchemeAsync(
            schemeName: "google-test",
            authority: "https://accounts.google.com",
            clientId: "978248021337-4h6k77vcouc9r4dihb9su29540d4m0ki.apps.googleusercontent.com",
            clientSecret: "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"
        );

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