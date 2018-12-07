using ParsiCoin.Base.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin
{
    public class MerkleTree
    {
        public class MerkleNode
        {
            public Guid ID { get; set; } = Guid.NewGuid();
            public string PubKey { get; set; }
            public double Balance { get; set; }
            public string HashString { get; set; }
            public string ComputeObjectHash()
                => $"{ID}-{PubKey}-{Balance}".ComputeHashString();
        }
        public MerkleNode[] Leafs { get; set; }
        public string[] Nodes { get; set; }
        public string Root { get => Nodes[0]; }
        public MerkleTree()
        {
            Leafs = new MerkleNode[65536];
            Nodes = new string[131071];
            for (int i = 0; i < Leafs.Length; i++)
            {
                Leafs[i] = new MerkleNode();
                Leafs[i].HashString = Leafs[i].ComputeObjectHash();
                Nodes[i + Nodes.Length / 2] = Leafs[i].HashString;
            }
            var Start = Nodes.Length / 2;
            var End = Nodes.Length;
            while (true)
            {
                if (Start == 0) break;
                for (int i = Start; i < End; i += 2)
                {
                    Nodes[(i) / 2] = $"{Nodes[i]}{Nodes[i + 1]}".ComputeHashString();
                }
                End = Start;
                Start /= 2;
            }

        }
    }
}
