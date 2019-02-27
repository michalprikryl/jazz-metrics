using Library.Security;
using System;

namespace JazzMetrics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(JwtKeyGenerator.GenerateJwtKey());

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
