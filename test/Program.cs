using System;
using System.Diagnostics;
using System.Security.Cryptography;
using ParsiCoin.Base.SecureLine.Server;
using ParsiCoin.Base.SecureLine.Client;
using ParsiCoin.Base.Utilities;
using ParsiCoin.Base.Crypto;
using ParsiCoin.PVM;
using ParsiCoin;
using System.Numerics;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            var k = new ECDSA();
            var k2 = new ECDSA();
            Util.PrivateKey = k.GetWords;
            var tx = new ParsiCoin.Transaction(k2.ExportPubKey, 10);
            Console.WriteLine(tx.ISSigntureVerified());

            //var s = new Stopwatch();
            //s.Start();
            //int j = 0;
            //for (int i = 0; i < int.MaxValue; i++)
            //{
            //    if ($"abcd-{i}".ComputeHash().CompareDiff()) Console.WriteLine($"{i}: {s.ElapsedMilliseconds} {(s.ElapsedMilliseconds / ++j)}");
            //}
            Console.ReadKey();
        }
    }
}
