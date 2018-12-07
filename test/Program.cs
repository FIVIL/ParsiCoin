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
using System.IO;
using ParsiCoin.Base;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Stopwatch();
            s.Start();
            var m = new MerkleTree();
            s.Stop();
            Console.WriteLine(s.Elapsed);
            Console.WriteLine(m.Root);
            Console.ReadKey();
        }
    }
}
