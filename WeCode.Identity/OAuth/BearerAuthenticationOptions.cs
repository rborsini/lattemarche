using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeCode.Identity.OAuth
{
    public class BearerAuthenticationOptions : JwtBearerAuthenticationOptions
    {

        public BearerAuthenticationOptions(string issuer, string secretKey)
            : base()
        {
            AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active;
            AllowedAudiences = new[] { "Any" };
            IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[] { 
                new SymmetricKeyIssuerSecurityKeyProvider(issuer, TextEncodings.Base64Url.Decode(secretKey)) 
            };
        }
    }
}
