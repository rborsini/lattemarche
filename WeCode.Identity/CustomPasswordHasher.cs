using Microsoft.AspNet.Identity;
using RB.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeCode.Identity
{
    public class CustomPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return new HashHelper().HashPassword(password);
        }


        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == HashPassword(providedPassword))
                return PasswordVerificationResult.Success;
            else
                return PasswordVerificationResult.Failed;
        }
    }
}
