using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeCode.Identity
{
    public class CustomSignInManager<TStore> : SignInManager<CustomUser, string>
        where TStore : IUserStore<CustomUser>, new()
    {
        public CustomSignInManager(CustomUserManager<TStore> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        { }

        public static CustomSignInManager<TStore> Create(IdentityFactoryOptions<CustomSignInManager<TStore>> options, IOwinContext context)
        {
            return new CustomSignInManager<TStore>(context.GetUserManager<CustomUserManager<TStore>>(), context.Authentication);
        }
    }
}
