using Library.Jazz;
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

            int? num = null;
            num++;

            string values = "Reviewed;Under construction;";
            string[] columns = values.Split(';');

            string url = string.Empty;
            //url = "https://158.196.141.113/rs/query/6/dataservice?report=6&limit=-1&basicAuthenticationEnabled=true"; //number - vice
            //url = "https://158.196.141.113/rs/query/1/dataservice?report=1&limit=-1&basicAuthenticationEnabled=true"; //number - jedno
            //url = "https://158.196.141.113/rs/query/7/dataservice?report=7&limit=-1&basicAuthenticationEnabled=true"; //coverage - vice druhu (HWRS, SWRS, SYRS)

            //NUMBER
            //url = "https://158.196.141.113/rs/query/6/dataservice?report=6&limit=-1&basicAuthenticationEnabled=true"; //prvni
            //url = "https://158.196.141.113/rs/query/1/dataservice?report=1&limit=-1&basicAuthenticationEnabled=true"; //treti
            //url = "https://158.196.141.113/rs/query/110/dataservice?report=102&limit=-1&basicAuthenticationEnabled=true"; //ctvrta
            //url = "https://158.196.141.113/rs/query/5/dataservice?report=5&limit=-1&basicAuthenticationEnabled=true"; //pata
            //url = "https://158.196.141.113/rs/query/3/dataservice?report=3&limit=-1&basicAuthenticationEnabled=true"; //sesta
            //url = "https://158.196.141.113/rs/query/4/dataservice?report=4&limit=-1&basicAuthenticationEnabled=true"; //desata

            //COVERAGE
            //url = "https://158.196.141.113/rs/query/17/dataservice?report=17&limit=-1&basicAuthenticationEnabled=true";
            //url = "https://158.196.141.113/rs/query/8/dataservice?report=8&limit=-1&basicAuthenticationEnabled=true";
            //url = "https://158.196.141.113/rs/query/15/dataservice?report=15&limit=-1&basicAuthenticationEnabled=true";
            //url = "https://158.196.141.113/rs/query/2/dataservice?report=2&limit=-1&basicAuthenticationEnabled=true";
            //url = "https://158.196.141.113/rs/query/16/dataservice?report=16&limit=-1&basicAuthenticationEnabled=true";
            url = "https://158.196.141.113/rs/query/7/dataservice?report=7&limit=-1&basicAuthenticationEnabled=true";

            JazzService jazz = new JazzService();
            //var task = jazz.CreateSnapshot(url, "mprikryl", "heslo");
            //task.Wait();
            //Console.WriteLine(task.Result);

            Console.WriteLine("\nEND");
            Console.ReadKey();
        }
    }
}
