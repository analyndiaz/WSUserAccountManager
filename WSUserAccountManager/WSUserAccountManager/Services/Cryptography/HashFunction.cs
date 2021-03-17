using System;
using System.Security.Cryptography;
using System.Text;
using WSUserAccountManager.Abstractions;

namespace WSUserAccountManager.Services.Cryptography
{
    public class HashFunction : IHashFunction
    {
        public string GetHashValue(string secretKey, string message)
        {
            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }

            Encoding encoding = Encoding.UTF8;
            using (var hmacsha256 = new HMACSHA256(encoding.GetBytes(secretKey)))
            {
                var hashedValue = hmacsha256.ComputeHash(encoding.GetBytes(message));
                return BitConverter.ToString(hashedValue).Replace("-", string.Empty).ToLower();
            }
        }
    }
}
