using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Identity.Interfaces;

namespace WeCode.Identity.OAuth
{
    /// <summary>
    /// Provide OAuth special options
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerOptions" />
    public class AuthorizationOptions<TStore> : OAuthAuthorizationServerOptions
        where TStore : IUserStore<CustomUser>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationOptions"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="tokeUrl">The toke URL.</param>
        public AuthorizationOptions(string tokenUrl, string issuer, string secretKey, IClaimsProvider claimsProvider)
        {
            TokenEndpointPath = new Microsoft.Owin.PathString(tokenUrl);
            AccessTokenExpireTimeSpan = SecurityConstants.TokenExpireTimeSpan;
            AccessTokenFormat = new JwtFormat<TStore>(this, issuer, secretKey);
            Provider = new AuthorizationProvider<TStore>(claimsProvider);
            AllowInsecureHttp = true;
        }

    }
}
