using ParsiCoin.Base.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace ParsiCoin
{
    public class MerkleTree
    {
        //public class MerkleNode
        //{
        //    public Guid ID { get; set; } = Guid.NewGuid();
        //    public string PubKey { get; set; }
        //    public double Balance { get; set; }
        //    public string HashString { get; set; }
        //    public string ComputeObjectHash()
        //        => $"{ID}-{PubKey}-{Balance}".ComputeHashString();
        //}
        public class TreeNode
        {
            public string Data { get; set; }
            public int ID { get; set; }
            public Guid Pointer { get; set; }
            public TreeNode(string d, Guid p)
            {
                Data = d;
                ID = 0;
                Pointer = p;
            }
            public TreeNode()
            {

            }
        }
        public Account[] Leafs { get; set; }
        public TreeNode[] Nodes { get; set; }
        public TreeNode Root { get => Nodes[0]; }

        public MerkleTree(TreeNode[] nodes)
        {
            Nodes = nodes;
        }
        public TreeNode[] UpdateTree(Account ac)
        {
            var i = Nodes.FirstOrDefault(x => x.Pointer == ac.ID).ID - 1;
            Nodes[i].Data = ac.HashStirng;
            while (i != 0)
            {
                if (i % 2 == 0)
                {
                    i--;
                }
                Nodes[i / 2].Data = $"{Nodes[i].Data}{Nodes[i + 1].Data}".ComputeHashString();
                i /= 2;

            }
            return Nodes;
        }
        public MerkleTree()
        {
            Leafs = new Account[65536];
            Nodes = new TreeNode[131071];
            //Leafs = new Account[4];
            //Nodes = new TreeNode[7];
            for (int i = 0; i < Leafs.Length; i++)
            {
                Leafs[i] = new Account(Guid.NewGuid());
                Leafs[i].HashStirng = Leafs[i].ComputeObjectHash();
                Nodes[i + Nodes.Length / 2] = new TreeNode
                    (Leafs[i].HashStirng, Leafs[i].ID);
            }
            var Start = Nodes.Length / 2;
            var End = Nodes.Length;
            while (true)
            {
                if (Start == 0) break;
                for (int i = Start; i < End; i += 2)
                {
                    Nodes[i / 2] = new TreeNode
                        ($"{Nodes[i].Data}{Nodes[i + 1].Data}".ComputeHashString(), Guid.Empty);
                }
                End = Start;
                Start /= 2;
            }
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].ID = i + 1;
            }
            Services.db.AddTree(Nodes);
            Services.db.AddAccountRange(Leafs);
        }


    }
}
