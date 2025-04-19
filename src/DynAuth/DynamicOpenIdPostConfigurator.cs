using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace DynAuth;

public class DynamicOpenIdPostConfigurator : IConfigureNamedOptions<OpenIdConnectOptions>
{
    private readonly IDataProtectionProvider _dataProtection;

    public DynamicOpenIdPostConfigurator(IDataProtectionProvider dataProtection)
    {
        _dataProtection = dataProtection;
    }

    public void Configure(string name, OpenIdConnectOptions options)
    {
        if (string.IsNullOrEmpty(options.SignInScheme))
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }

        if (options.StateDataFormat == null)
        {
            var protector = _dataProtection.CreateProtector(
                typeof(OpenIdConnectHandler).FullName!, name, "v1");
            options.StateDataFormat = new PropertiesDataFormat(protector);
        }

        if (options.StringDataFormat == null)
        {
            var protector = _dataProtection.CreateProtector(
                typeof(OpenIdConnectHandler).FullName!, typeof(string).FullName!, name, "v1");
            options.StringDataFormat = new SecureDataFormat<string>(new StringSerializer(), protector);
        }

        if (string.IsNullOrEmpty(options.TokenValidationParameters.ValidAudience) &&
            !string.IsNullOrEmpty(options.ClientId))
        {
            options.TokenValidationParameters.ValidAudience = options.ClientId;
        }
    }

    public void Configure(OpenIdConnectOptions options) => Configure(Options.DefaultName, options);
    
    private sealed class StringSerializer : IDataSerializer<string>
    {
        public string Deserialize(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public byte[] Serialize(string model)
        {
            return Encoding.UTF8.GetBytes(model);
        }
    }
}
