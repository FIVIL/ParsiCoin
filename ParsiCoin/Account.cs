using ParsiCoin.Base;
using ParsiCoin.Base.Crypto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin
{
    public class Account : IPICObject
    {
        public Guid ID { get; private set; }
        private readonly ECDSA signtureProvider;
        public string GetPubKey { get => signtureProvider.ExportPubKey; }
        public double Balance { get; set; }
        public List<Node> InCome { get; set; }
        public List<Node> OutGo { get; set; }
        public string HashStirng { get; set; }

        public Account(string pKey)
        {
            signtureProvider = new ECDSA(pKey, false);
        }
        public string ComputeObjectHash()
        {
            throw new NotImplementedException();
        }

        public bool Equal(IPICObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
