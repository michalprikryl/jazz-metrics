using Library.Security;
using System;
using System.Globalization;

namespace JazzMetrics
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(JwtKeyGenerator.GenerateJwtKey());

            string name = "libor";
            string n = PasswordHelper.Base64Encode(name);
            Console.WriteLine(n);
            string r = PasswordHelper.Base64Decode(n);
            Console.WriteLine(r);

            Console.WriteLine(DateTime.Now.ToString("MMM dd, yyyy hh:mm:ss tt", CultureInfo.GetCultureInfo("en")));

            Console.ReadKey();
        }
    }
}
