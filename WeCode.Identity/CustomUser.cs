using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WeCode.Identity
{
    public class CustomUser : IUser<string>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public List<string> Roles { get; set; }

        public List<string> Permissions { get; set; }

        public bool BackOffice { get; set; }
        public string Tenant {  get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<CustomUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
