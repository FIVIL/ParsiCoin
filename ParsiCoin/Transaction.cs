using ParsiCoin.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ParsiCoin.Base.Utilities;
using ParsiCoin.Base.Crypto;
using Newtonsoft.Json;
using ParsiCoin.PVM;

namespace ParsiCoin
{
    public class Transaction : IPICObject
    {
        public string TransactionIssuer { get; private set; }
        public string Reciepient { get; private set; }
        public double Amount { get; private set; }

        public DateTime IsuueTime { get; private set; }
        public DateTime ApprovalTime { get; set; }

        public string TxHash { get; private set; }

        [StringLength(100)]
        public string TxMessage { get; private set; }

        public Guid NodeID { get; set; }

        public string Signture { get; private set; }

        public string ScriptPubKey { get; private set; }
        public string ScriptSig { get; private set; }

        #region ctor
        public Transaction(string reciepient, double amount, string message = "")
        {
            var ec = new ECDSA(Util.PrivateKey, false);
            TransactionIssuer = ec.ExportPubKey;
            Reciepient = reciepient;
            Amount = amount;
            IsuueTime = DateTime.UtcNow;
            TxHash = ComputeTxHash();
            //Signture = ec.Sign(TxHash).ToByteArray(StringEncoding.Base64).ToBase58Check();
            Signture = ec.Sign(TxHash);
            ScriptPubKey = $"{TxHash};{Signture};{TransactionIssuer}";
            ScriptSig = $"{ScriptPubKey};CheckSig;IsOne";
        }
        [JsonConstructor]
        public Transaction()
        {

        }
        #endregion
        public bool ISSigntureVerified()
        {
            if (TxHash == ComputeTxHash())
            {
                //var ecdsa = new ECDSA(TransactionIssuer);

                //return ecdsa.Verify(Signture.ToByteArray(StringEncoding.Base85Check).ToBase64(), TxHash);

                var machine = new PUnite();
                machine.Parser(ScriptSig);
                return machine.Process() ?? false;
            }
            return false;
        }
        public string ComputeTxHash()
            => $"{TransactionIssuer}-{Reciepient}-{Amount}-{IsuueTime}".ComputeHashString();
    }
}
