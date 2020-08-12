using LatteMarche.Identity;
using LatteMarche.Identity.OAuth;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WeCode.Identity;
using WeCode.Identity.OAuth;

[assembly: OwinStartup(typeof(LatteMarche.WebApi.Startup))]
namespace LatteMarche.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            // Add our custom managers
            app.CreatePerOwinContext<CustomUserManager<CustomUserStore>>(CustomUserManager<CustomUserStore>.Create);
            app.CreatePerOwinContext<CustomSignInManager<CustomUserStore>>(CustomSignInManager<CustomUserStore>.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Token Generation
            var issuer = ConfigurationManager.AppSettings["jwtIssuer"];
            var secretKey = ConfigurationManager.AppSettings["jwtSecretKey"];
            var claimsProvider = new ClaimsProvider();

            app.UseOAuthAuthorizationServer(new AuthorizationOptions<CustomUserStore>("/Token", issuer, secretKey, claimsProvider));
            app.UseJwtBearerAuthentication(new BearerAuthenticationOptions(issuer, secretKey));

        }
    }
}