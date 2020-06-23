using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Identity.OAuth
{
    public static class SecurityConstants
    {
        public static TimeSpan TokenExpireTimeSpan = TimeSpan.FromDays(7);

        /// <summary>
        /// Gets the signature algorithm.
        /// </summary>
        /// <value>
        /// The signature algorithm.
        /// </value>
        public static string SignatureAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";

        /// <summary>
        /// Gets the digest algorithm.
        /// </summary>
        /// <value>
        /// The digest algorithm.
        /// </value>
        public static string DigestAlgorithm = "http://www.w3.org/2001/04/xmlenc#sha256";

        /// <summary>
        /// The private key to token cryptography (shared with labview and web) 
        /// </summary>
        public static string SecretKey = "Q6Ep%]J544uKudi7&iJ%9.nT~b}AQp";

        /// <summary>
        /// The audience
        /// </summary>
        public static string Audience = "all";

        /// <summary>
        /// The issuer
        /// </summary>
        public static string Issuer = "localhost";
    }
}