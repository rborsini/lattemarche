using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeCode.Identity.OAuth
{
    public class BearerAuthenticationProvider : OAuthBearerAuthenticationProvider
    {

        public override Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            context.Validated();
            return base.ValidateIdentity(context);
        }

    }
}
