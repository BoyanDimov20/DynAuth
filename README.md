# 🔐 Dynamic authentication with OpenIDConnect & SAML Schemes in ASP.NET Core

DynAuth is a flexible authentication library for .NET applications, providing support for multiple authentication protocols including SAML2 and OpenID Connect. This is useful for multi-tenant apps or when your identity provider setup is not static.

## Features

- 🔐 Multi-protocol support
  - SAML2 authentication
  - OpenID Connect integration
- 🎯 Built for .NET 8.0
- ⚡ Easy integration with ASP.NET Core applications
- 🛠️ Customizable authentication flows

---

## 🧩 Adding a Schemes at Runtime

### 1. `IOidcSchemeManager`
Call this from anywhere (e.g., an admin panel or per-tenant middleware):
- Adds new oidc client on the fly
  
```csharp
public class SchemeService
{
    public Task AddScheme()
    {
        var options = new OpenIdConnectOptions
        {
            SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,
            Authority = "https://accounts.google.com",
            ClientId = "*",
            ClientSecret = "*",
            ResponseType = "code",
            SaveTokens = true,
            CallbackPath = $"/signin-google-test",
        };
        
        _oidcSchemeManager.AddScheme("google-test", options);
    }
}
```

### 2. `ISamlSchemeManager`
Call this from anywhere (e.g., an admin panel or per-tenant middleware):
- Adds new saml client on the fly

```csharp
public class SchemeService
{
    public Task AddScheme()
    {
        var options = new Saml2Options
        {
            SPOptions =
            {
                EntityId = new EntityId("https://localhost:7119"),
                ModulePath = "/signin-azure-saml2"
            }
        };
        
        var idp = new IdentityProvider(new EntityId("https://sts.windows.net/16e2eac8-c69c-4976-919b-test/"),
            options.SPOptions)
        {
            Binding = Saml2BindingType.HttpPost,
            LoadMetadata = true,
            MetadataLocation = "https://login.microsoftonline.com/16e2eac8-c69c-4976-919b-4c3a48c2c0f7/federationmetadata/2007-06/federationmetadata.xml?appid=b88b09ee-52b4-4454-8c1f-test"
        };

        options.IdentityProviders.Add(idp);
        
        _samlSchemeManager.AddScheme("azure", options);
    }
}
```

## 🛠️ Service Registration
In `Program.cs`:

```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddDynAuthOpenIdConnect()
    .AddDynAuthSaml();
```

---

## 🧪 Testing the Setup
Check out the `samples` directory for example implementations and usage.

---

## 🧩 Want to Contribute?
Feel free to fork and enhance this with:
- Admin UI for managing schemes
- Database persistence
- Caching / refreshing tokens

---

## 📬 Questions / Help?
Open an issue or discussion if you need help wiring this up in your own project.


