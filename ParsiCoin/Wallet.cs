using ParsiCoin.Base.Crypto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin
{
    public class Wallet
    {
        private readonly ECDSA signtureProvider;
        public string GetPubKey { get => signtureProvider.ExportPubKey; }
        public Wallet(string Pkey)
        {
            signtureProvider = new ECDSA(Pkey, false);
        }
    }
}
