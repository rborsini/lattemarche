using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Identity.OAuth
{

    public class OAuthProvider : OAuthAuthorizationServerProvider
    {


        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<CustomUserManager>();

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            var owinUser = context.OwinContext.Get<CustomUser>(context.UserName);

            var user = userManager.FindAsync(context.UserName, context.Password).Result;

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return Task.FromResult(1);
            }

            identity.AddClaim(new Claim(LatteMarcheClaimTypes.username.ToString(), user.UserName));
            identity.AddClaim(new Claim(LatteMarcheClaimTypes.displayName.ToString(), user.DisplayName));
            identity.AddClaim(new Claim(LatteMarcheClaimTypes.roles.ToString(), String.Join("|", user.Roles)));
            identity.AddClaim(new Claim(LatteMarcheClaimTypes.permissions.ToString(), String.Join("|", user.Permissions)));

            context.Validated(identity);

            ClaimsIdentity cookiesIdentity = userManager.CreateIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType).Result;
            context.Request.Context.Authentication.SignIn(cookiesIdentity);

            return Task.FromResult(0);
        }


        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

    }
}
