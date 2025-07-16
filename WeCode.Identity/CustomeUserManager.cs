using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeCode.Identity
{
    public class CustomUserManager<TStore> : UserManager<CustomUser>
        where TStore : IUserStore<CustomUser>, new()
    {
        public CustomUserManager(TStore store)
            : base(store)
        {
            this.PasswordHasher = new CustomPasswordHasher();
        }

        public static CustomUserManager<TStore> Create()
        {
            return new CustomUserManager<TStore>(new TStore());
        }
    }
}
