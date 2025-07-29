using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace ManagmentSystemCertificate
{
    internal class PasswordHashed
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password ,string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
