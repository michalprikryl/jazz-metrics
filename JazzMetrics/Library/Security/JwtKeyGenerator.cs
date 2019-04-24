using System;
using System.Security.Cryptography;

namespace Library.Security
{
    /// <summary>
    /// generuje klic pro JWT
    /// </summary>
    public static class JwtKeyGenerator
    {
        /// <summary>
        /// generuje klic pro JWT
        /// </summary>
        /// <returns></returns>
        public static string GenerateJwtKey()
        {
            HMACSHA256 hmac = new HMACSHA256();
            return Convert.ToBase64String(hmac.Key);
        }
    }
}
