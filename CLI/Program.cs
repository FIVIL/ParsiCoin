using ParsiCoin;
using System;
using System.Diagnostics;
using System.Linq;
namespace CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Services.Init("Hamed");
            var m = new MerkleTree();



            Console.ReadKey();
        }
    }
}
