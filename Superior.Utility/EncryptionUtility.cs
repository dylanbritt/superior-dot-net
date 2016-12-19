using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Superior.Utility
{
    public static class EncryptionUtility
    {
        private const int PBKDF2_DEFAULT_ITERATION_COUNT = 262144;
        private const int PBKDF2_DEFAULT_DERIVED_KEY_LENGTH = 256;

        // TODO: Unit Test
        public static byte[] GenerateRandom32ByteSalt()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[32];
            rnd.NextBytes(b);
            return b;
        }

        // TODO: Unit Test
        public static byte[] GenerateRandom64ByteSalt()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[64];
            rnd.NextBytes(b);
            return b;
        }

        // TODO: Unit Test
        public static byte[] SHA512HashString(string str, byte[] salt = null)
        {
            var valueToHash = string.IsNullOrEmpty(str) ? string.Empty : str;

            var csp = new SHA512CryptoServiceProvider();

            return csp.ComputeHash(Encoding.ASCII.GetBytes(valueToHash).Concat(salt).ToArray());
        }

        // TODO: Unit Test
        public static byte[] PBKDF2(string password,
                                    byte[] salt = null,
                                    int iterationCount = PBKDF2_DEFAULT_ITERATION_COUNT,
                                    int derivedKeyLength = PBKDF2_DEFAULT_DERIVED_KEY_LENGTH)
        {
            byte[] hashValue;
            var valueToHash = string.IsNullOrEmpty(password) ? string.Empty : password;
            using (var pbkdf2 = new Rfc2898DeriveBytes(valueToHash, salt, iterationCount))
            {
                hashValue = pbkdf2.GetBytes(derivedKeyLength);
            }
            return hashValue;
        }
    }
}
