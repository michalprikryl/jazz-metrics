using System;
using System.Security.Cryptography;

namespace Library.Security
{
    public static class JwtKeyGenerator
    {
        public static string GenerateJwtKey()
        {
            HMACSHA256 hmac = new HMACSHA256();
            return Convert.ToBase64String(hmac.Key);
        }
    }
}
