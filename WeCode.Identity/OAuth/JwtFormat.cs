using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeCode.Identity.OAuth
{
    /// <summary>
    /// Provide Token format
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.ISecureDataFormat{Microsoft.Owin.Security.AuthenticationTicket}" />
    public class JwtFormat<TStore> : ISecureDataFormat<AuthenticationTicket>
        where TStore : IUserStore<CustomUser>, new()
    {

        private readonly AuthorizationOptions<TStore> options;
        private string issuer;
        private string secretKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtFormat"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public JwtFormat(AuthorizationOptions<TStore> options, string issuer, string secretKey)
        {
            this.options = options;
            this.issuer = issuer;
            this.secretKey = secretKey;
        }


        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;
            var secret = TextEncodings.Base64Url.Decode(this.secretKey);
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(secret);
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            var jwtSecurityToken = new JwtSecurityToken(this.issuer, "Any", data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }

    }
}
