using System;
using System.Diagnostics;
using System.Security.Cryptography;
using ParsiCoin.Base.SecureLine.Server;
using ParsiCoin.Base.SecureLine.Client;
using ParsiCoin.Base.Utilities;
using ParsiCoin.Base.Crypto;

namespace test
{
    class Program
    {
        private static int s = 0;
        private static int update(int i) => s = i;
        static void Main(string[] args)
        {
            Console.WriteLine(s);
            s++;
            Console.WriteLine(s);
            Console.WriteLine(update(10));
            Console.WriteLine(s);
            Console.ReadKey();
        }
    }
}
