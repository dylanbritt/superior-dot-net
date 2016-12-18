using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Superior.Utility
{
    public static class EncryptionUtility
    {
        public static byte[] Generate32ByteSalt()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[32];
            rnd.NextBytes(b);
            return b;
        }

        public static byte[] SHA512HashString(string str, byte[] salt = null)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            var csp = new SHA512CryptoServiceProvider();

            return csp.ComputeHash(Encoding.ASCII.GetBytes(str).Concat(salt).ToArray());
        }
    }
}
