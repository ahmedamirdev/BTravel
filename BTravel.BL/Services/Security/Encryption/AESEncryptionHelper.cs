using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.BL.Services.Security.Encryption
{
    public static class AESEncryptionHelper
    {
        private static readonly byte[] _key;
        private static readonly byte[] _iv;

        static AESEncryptionHelper()
        {
            string keyString = "mySuperSecretKey123456";  // Change this to a strong key
            string ivString = "myInitVector123456";       // Change this to a strong IV

            // Convert to 32-byte key for AES-256
            _key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(keyString));

            // Convert IV to 16 bytes
            _iv = Encoding.UTF8.GetBytes(ivString.PadRight(16).Substring(0, 16));
        }

        /// <summary>
        /// Encrypts a string and returns a Base64-encoded ciphertext.
        /// </summary>
        public static string Encrypt(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext)) throw new ArgumentNullException(nameof(plaintext));

            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(cs))
                    {
                        writer.Write(plaintext);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypts a Base64-encoded string back to plaintext.
        /// </summary>
        public static string Decrypt(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext)) throw new ArgumentNullException(nameof(ciphertext));

            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(ciphertext)))
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}