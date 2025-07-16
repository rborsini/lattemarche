using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WeCode.Identity.Interfaces
{
    public interface IClaimsProvider
    {
        List<Claim> GetClaims(CustomUser user);
    }
}
