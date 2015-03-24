using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeddingInvitation.Infrastructure.Security
{
    public class PasswordHelper
    {
        public const int DEFAULT_SALT_SIZE = 16;

        public static string CreatePasswordSalt(int size)
        {
            string result = string.Empty;
            System.Security.Cryptography.RNGCryptoServiceProvider provider = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] bytes = new byte[size];
            provider.GetBytes(bytes);
            result = Convert.ToBase64String(bytes);
            return result;
        }

        public static string CreatePasswordHash(string password, string passwordSalt)
        {
            string result = string.Empty;
            string passwordAndSalt = string.Concat(password, passwordSalt);
            byte[] bytes = System.Text.UnicodeEncoding.Unicode.GetBytes(passwordAndSalt);
            System.Security.Cryptography.SHA1 provider = new System.Security.Cryptography.SHA1Managed();
            bytes = provider.ComputeHash(bytes);
            result = Convert.ToBase64String(bytes);
            return result;
        }
    }
}
