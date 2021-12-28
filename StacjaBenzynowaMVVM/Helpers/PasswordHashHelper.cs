using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace StacjaBenzynowaMVVM.Helpers
{
    public class PasswordHashHelper
    {
        public static string HashPassword(string password)
        {
            byte[] pass = Encoding.ASCII.GetBytes(password);
            byte[] hashValue;
            using (SHA256 mySHA256 = SHA256.Create())
            {
                hashValue = mySHA256.ComputeHash(pass);
            }
            return Encoding.ASCII.GetString(hashValue);
        }
    }
}
