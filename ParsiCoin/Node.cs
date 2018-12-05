using ParsiCoin.Base;
using ParsiCoin.Base.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ParsiCoin
{
    public class Node : IPICObject
    {
        public Guid ID { get; private set; }
        public DateTime MintTime { get; private set; }
        public DateTime PublichTime { get; private set; }

        public Guid Left { get; private set; }
        public Guid Right { get; private set; }
        public string LeftHash { get; private set; }
        public string RightHash { get; private set; }

        [StringLength(100)]
        public string Message { get; set; }

        public Transaction Tx { get; set; }

        public string TxHash { get; set; }

        public string IssuerPubKey { get; set; }
        public string NodeHash { get; set; }

        public bool Equal(IPICObject obj)
            => this.ToJson().ComputeHashString().Equals(obj.ToJson().ComputeHashString());
    }
}
