﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace Library.Security
{
    /// <summary>
    /// trida pro praci s hesly
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// generuje nahodny string o dane delce (vhodne pro password nebo salt)
        /// </summary>
        /// <param name="length">pozadovana delka retezce</param>
        /// <returns></returns>
        public static string GeneratePassword(int length)
        {
            const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            Random randNum = new Random();
            char[] chars = new char[length];
            int countOfCharacters = characters.Length;
            for (var i = 0; i < length; i++)
            {
                chars[i] = characters[randNum.Next(0, countOfCharacters)];
            }

            return new string(chars);
        }

        /// <summary>
        /// zasifruje do MD5 password spolu se salt - vhodne pro ulozeni do db, jen je nutne mit tam i ulozenou salt v puvodni podobe
        /// </summary>
        /// <param name="pass">heslo</param>
        /// <param name="salt">salt z DB nebo tak</param>
        /// <returns></returns>
        public static string EncodePassword(string pass, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);

            //return Convert.ToBase64String(inArray);    
            return EncodePasswordMd5(Convert.ToBase64String(inArray));
        }

        /// <summary>
        /// vytvori MD5 hash ze stringu
        /// </summary>
        /// <param name="pass">string</param>
        /// <returns></returns>
        private static string EncodePasswordMd5(string pass)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] originalBytes = Encoding.Default.GetBytes(pass);
            byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes);
        }

        /// <summary>
        /// vytvori tajny base64 z plaintextu
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText)).Reverse();

        /// <summary>
        /// vytvori plaintext z tajneho base64
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string Base64Decode(string base64EncodedData) => Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData.Reverse()));
    }
}
