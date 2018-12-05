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
            //var k = new ECDSA();
            //var k2 = new ECDSA();
            //Util.PrivateKey = k.GetWords;
            //var tx = new ParsiCoin.Transaction(k2.ExportPubKey, 10);
            //var tx2 = tx.ToJson().FromJson<Transaction>();
            //Node n = new Node(new Node(), new Node(), "aaa", k.ExportPubKey, tx);
            //var n2 = n.ToJson().FromJson<Node>();
            //Console.WriteLine(n.ToJson(true));
            //Console.WriteLine(n.Equal(n2));
            for (int i = 0; i < int.MaxValue; i++)
            {
                if ($"abcd-{i}".ComputeHash().CompareDiff()) Console.WriteLine($"{i}: {$"abcd-{i}".ComputeHash().ToBase58Check()}");
            }
            Console.ReadKey();
        }
    }
}
