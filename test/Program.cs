using System;
using System.Diagnostics;
using System.Security.Cryptography;
using ParsiCoin.Base.SecureLine.Server;
using ParsiCoin.Base.SecureLine.Client;
using ParsiCoin.Base.Utilities;
using ParsiCoin.Base.Crypto;
using ParsiCoin.PVM;
namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var s = new PUnite();
            //s.Push("Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed ".ComputeHash().FromByteArray());
            //s.Push("Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed ");
            //s.AddCommand(Commands.DoubleSHA512);
            //s.AddCommand(Commands.Eq);
            //s.AddCommand(Commands.IsOne);
            s.Parser($"{"Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed ".ComputeHash().ToBase58Check()};{"Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed Hamed ".ComputeHash().ToBase58Check()};Eq;IsOne");
            Console.WriteLine(s.Process());
            Console.ReadKey();
        }
    }
}
