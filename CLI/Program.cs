using ParsiCoin;
using System;
using System.Diagnostics;

namespace CLI
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
