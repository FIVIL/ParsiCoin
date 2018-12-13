using Newtonsoft.Json;
using ParsiCoin.Base;
using ParsiCoin.Base.Crypto;
using ParsiCoin.Base.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin
{
    public class Account : IPICObject
    {
        public Guid ID { get; private set; }

        [JsonIgnore]
        private readonly ECDSA signtureProvider;

        private string pKey;
        public string GetPubKey
        {
            get =>
                !(signtureProvider is null) ? signtureProvider.ExportPubKey : pKey;
            set => pKey = value;
        }
        public double Balance { get; set; }
        [JsonIgnore]
        public List<Node> InCome { get; set; }
        [JsonIgnore]
        public List<Node> OutGo { get; set; }
        public string HashStirng { get; set; }

        #region ctor
        public Account(string pKey, Guid id)
        {
            ID = id;
            signtureProvider = new ECDSA(pKey, false);
            //
            Balance = 0;
            InCome = new List<Node>();
            OutGo = new List<Node>();
            //
            HashStirng = ComputeObjectHash();
        }
        [JsonConstructor]
        public Account(Guid id, string getPubKey, double balance, string hashStirng)
        {
            ID = id;
            GetPubKey = getPubKey;
            Balance = balance;
            HashStirng = hashStirng;
        }
        public Account()
        {

        }
        public Account(Guid id)
        {
            ID = id;
            pKey = string.Empty;
            Balance = 0;
        }
        #endregion
        public Transaction TransactionBuilder(string reciepient, double value, string message = "")
        {
            if (value > Balance) throw new Exception("Not enough funds.");
            var t = new Transaction(reciepient, value, signtureProvider, message);
            if (t.ISSigntureVerified()) return t;
            throw new Exception("Something went wrong, cannot sign the transaction");
        }
        public string ComputeObjectHash()
            => $"{ID}-{GetPubKey}-{Balance}".ComputeHashString();

        public bool Equal(IPICObject obj)
        {
            var acc = obj as Account;
            if (acc.HashStirng != acc.ComputeObjectHash()) return false;
            return HashStirng == acc.HashStirng;
        }
        public string SignMessage(string message) => signtureProvider.Sign(message);
        public bool IsSignVerified(string message, string sign) => signtureProvider.Verify(sign, message);
    }
}
