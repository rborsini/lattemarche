using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Identity.OAuth
{
    /// <summary>
    /// Provide OAuth special options
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerOptions" />
    public class OAuthOptions : OAuthAuthorizationServerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthOptions"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="tokeUrl">The toke URL.</param>
        public OAuthOptions(string tokeUrl)
        {
            TokenEndpointPath = new Microsoft.Owin.PathString(tokeUrl);
            AccessTokenExpireTimeSpan = SecurityConstants.TokenExpireTimeSpan;
            AccessTokenFormat = new JwtFormat(this);
            Provider = new OAuthProvider();
            AllowInsecureHttp = true;
        }

        public OAuthProvider AulosProvider
        {
            get { return Provider as OAuthProvider; }
        }
    }
}
