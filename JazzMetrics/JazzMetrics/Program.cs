using Library.Security;
using Library.Services.Jazz;
using System;
using System.Globalization;
using System.Linq;

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

            //string values = "Reviewed;Under construction;";
            //string[] columns = values.Split(';');

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

            //int[] values = new int[] { /*1, 2, 3, 4, 5, 6, 7, 8,*/ 9 };
            int[] values = new int[] { 1,2,3,5,4,3 };
            var lastThreeValues = values.Skip(Math.Max(values.Length - 3, 0)).ToArray();
            for (int i = 1; i < lastThreeValues.Length; i++)
            {
                if (lastThreeValues[i] >= lastThreeValues[i - 1])
                {
                    Console.WriteLine("ne");
                }
            }

            Console.WriteLine("ano");

            Console.WriteLine("\nEND");
            Console.ReadKey();
        }
    }
}
