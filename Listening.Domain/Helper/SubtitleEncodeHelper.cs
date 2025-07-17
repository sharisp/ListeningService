using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Listening.Domain.Helper
{
    public static class SubtitleEncodeHelper
    {
        public static string EncodeSubtitle(string subtitleContent)
        {
            string key = Guid.NewGuid().ToString("N")[..16];
            string iv = Guid.NewGuid().ToString("N")[..16];

            string encrypted = AESEncrypt(subtitleContent, key, iv);

            string obfuscatedEncrypted = Guid.NewGuid().ToString("N")[..16] + encrypted;

            var payload = new
            {
                key,
                iv,
                encrypted = obfuscatedEncrypted
            };

            string json = JsonSerializer.Serialize(payload);
            string base64Json = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            string final = Guid.NewGuid().ToString("N")[..10] + base64Json;

            return final;
        }



        public static string AESEncrypt(string plainText, string key, string iv)
        {
            if (key.Length != 16 || iv.Length != 16)
                throw new ArgumentException("Key and IV must be 16 characters (128 bits)");

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var encryptor = aes.CreateEncryptor();
            byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            return Convert.ToBase64String(encrypted);
        }

        public static string AESDecrypt(string base64CipherText, string key, string iv)
        {
            if (key.Length != 16 || iv.Length != 16)
                throw new ArgumentException("Key and IV must be 16 characters (128 bits)");

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] cipherBytes = Convert.FromBase64String(base64CipherText);
            var decryptor = aes.CreateDecryptor();
            byte[] decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(decrypted);
        }




    }
}
