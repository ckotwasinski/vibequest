using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace VibeQuest.Utility.Helpers
{
    /// <summary>
    /// Password-Based Key Derivation Function 2
    /// </summary>
    public class PBKDF2
    {
        private const int SaltByteSize = 24;
        private const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash 
        private const int Pbkdf2Iterations = 1000;
        private const int IterationIndex = 0;
        private const int SaltIndex = 1;
        private const int Pbkdf2Index = 2;

        private PBKDF2() { }

        /// <summary>
        /// Hash plain text password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Tuple<string, string> HashPassword(string password)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);

            return Tuple.Create(Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        /// <summary>
        /// Validate password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string password, string salt, string hash)
        {
            var saltByte = Convert.FromBase64String(salt);
            var hashByte = Convert.FromBase64String(hash);

            var testHash = GetPbkdf2Bytes(password, saltByte, Pbkdf2Iterations, hashByte.Length);
            return SlowEquals(hashByte, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }

    public class SHA256
    {
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            byte[] byteValue = Encoding.UTF8.GetBytes(input);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}
