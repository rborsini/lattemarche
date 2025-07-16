using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WeCode.Identity.Interfaces;

namespace WeCode.Identity.OAuth
{

    public class AuthorizationProvider<TStore> : OAuthAuthorizationServerProvider
        where TStore : IUserStore<CustomUser>, new()
    {

        #region Fields

        private IClaimsProvider claimsProvider;

        #endregion

        #region Constructor

        public AuthorizationProvider(IClaimsProvider claimsProvider)
        {
            this.claimsProvider = claimsProvider;
        }

        #endregion

        #region Methods

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<CustomUserManager<TStore>>();

            var user = userManager.FindAsync(context.UserName, context.Password).Result;
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return Task.FromResult(1);
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));

            foreach (var claim in this.claimsProvider.GetClaims(user))
                identity.AddClaim(claim);

            identity.AddClaim(new Claim("sub", context.UserName));

            context.Validated(identity);

            ClaimsIdentity cookiesIdentity = userManager.CreateIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType).Result;
            context.Request.Context.Authentication.SignIn(cookiesIdentity);

            return Task.FromResult(0);
        }

        #endregion

    }
}