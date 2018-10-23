﻿using System;
using WebAPI.Classes.Helpers;

namespace JazzMetrics
{
    class Program
    {
        static void Main(string[] args)
        {
            string password = "heslo";
            string salt = PasswordHelper.GeneratePassword(10);
            string generatedPassword = PasswordHelper.EncodePassword(password, salt);
            Console.WriteLine("passwd => {0} \nsalt => {1}", salt, generatedPassword);

            Console.WriteLine("END");
            Console.ReadKey();
        }
    }
}
