using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Identity
{
    public class CustomUserManager : UserManager<CustomUser>
    {
        public CustomUserManager(IUserStore<CustomUser> store)
            : base(store)
        {
            this.PasswordHasher = new CustomPasswordHasher();
        }

        public static CustomUserManager Create()
        {
            return new CustomUserManager(new CustomUserStore());
        }
    }
}
