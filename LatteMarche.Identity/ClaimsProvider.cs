using LatteMarche.Identity.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WeCode.Identity;
using WeCode.Identity.Interfaces;

namespace LatteMarche.Identity
{
    public class ClaimsProvider : IClaimsProvider
    {
        public List<Claim> GetClaims(CustomUser user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(LatteMarcheClaimTypes.username.ToString(), user.UserName));
            claims.Add(new Claim(LatteMarcheClaimTypes.displayName.ToString(), user.DisplayName));
            claims.Add(new Claim(LatteMarcheClaimTypes.roles.ToString(), String.Join("|", user.Roles)));
            claims.Add(new Claim(LatteMarcheClaimTypes.permissions.ToString(), String.Join("|", user.Permissions)));
            claims.Add(new Claim(LatteMarcheClaimTypes.tenant.ToString(), user.Tenant));

            return claims;
        }

    }
}
