using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Cryptography
{
    /// <summary>
    /// A helper class for Argon and SHA-256.
    /// </summary>
    public class CryptoHelper
    {
        /// <summary>
        /// Compute a sha-256 hash from a raw-string.
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static string ComputeSHA256(string rawData)
        {
            using (var sha = SHA256.Create())
            {
                var hashArray = sha.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                var s_builder = new StringBuilder();
                for (int i = 0; i < hashArray.Length; i++) s_builder.Append(hashArray[i].ToString("x2"));
                return s_builder.ToString();
            }
        }

        /// <summary>
        /// Compute an Argon-level hash from a raw-string.
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static string ArgonHash(string rawData)
        {
            // Research argon2 settings (specifically memorycost).
            return Argon2.Hash(rawData, 3, 256);
        }
        
        public static bool VerifyPassword(string dbGiven, string given)
        {
            return Argon2.Verify(dbGiven, given);
        }
    }
}
