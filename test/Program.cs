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
            //var k = new ECDSA();
            //var k2 = new ECDSA();
            //Util.Conf = new ParsiCoin.Base.Configurations();
            //Util.Conf.PrivateKey = k.GetWords;
            //var tx = new ParsiCoin.Transaction(k2.ExportPubKey, 10);
            //Console.WriteLine(tx.ISSigntureVerified());

            //var s = new Stopwatch();
            //s.Start();
            //int j = 0;
            //for (int i = 0; i < int.MaxValue; i++)
            //{
            //    if ($"abcd-{i}".ComputeHash().CompareDiff()) Console.WriteLine($"{i}: {s.ElapsedMilliseconds} {(s.ElapsedMilliseconds / ++j)}");
            //}
            Services.Init(Console.ReadLine());

            //Console.WriteLine(Util.Conf.ToJson(true));
            Console.WriteLine(Services.Wallet.PrimaryAccount.ToJson(true));
            var acc = Services.Wallet.PrimaryAccount.ToJson(true).FromJson<Account>();
            Console.WriteLine();
            //Console.WriteLine(acc.ToJson(true));
            //Console.WriteLine(Services.Wallet.PrimaryAccount.Equal(acc));
            //Services.db.AddAccount(acc);
            Console.WriteLine(Services.db.GetAccount(acc.ID).ToJson(true));
            Console.ReadKey();
        }
    }
}
