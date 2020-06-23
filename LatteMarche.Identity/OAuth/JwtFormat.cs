using Microsoft.Owin.Security;
using System;
using System.IdentityModel.Tokens;
using System.Text;

namespace LatteMarche.Identity.OAuth
{
    /// <summary>
    /// Provide Token format
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.ISecureDataFormat{Microsoft.Owin.Security.AuthenticationTicket}" />
    public class JwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly OAuthOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtFormat"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public JwtFormat(OAuthOptions options)
        {
            this.options = options;
        }


        /// <summary>
        /// Protects the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">data</exception>
        public string Protect(AuthenticationTicket data)
        {
            if (data == null) throw new ArgumentNullException("data");

            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(options.AccessTokenExpireTimeSpan.TotalMinutes);
            var signingCredentials = new SigningCredentials(
                                        new InMemorySymmetricSecurityKey(Encoding.ASCII.GetBytes(SecurityConstants.SecretKey)),
                                        SecurityConstants.SignatureAlgorithm,
                                        SecurityConstants.DigestAlgorithm);
            var token = new JwtSecurityToken(SecurityConstants.Issuer, SecurityConstants.Audience, data.Identity.Claims,
                                             now, expires, signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Remove protection to the specified protected text.
        /// </summary>
        /// <param name="protectedText">The protected text.</param>
        /// <returns></returns>
        public AuthenticationTicket Unprotect(string protectedText)
        {
            //throw new NotImplementedException();
            return null; //TODO
        }
    }
}