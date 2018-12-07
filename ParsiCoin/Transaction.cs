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
        public Transaction(string reciepient, double amount, ECDSA ec, string message = "")
        {
            TransactionIssuer = ec.ExportPubKey;
            Reciepient = reciepient;
            Amount = amount;
            IsuueTime = DateTime.UtcNow;
            TxHash = ComputeObjectHash();
            //Signture = ec.Sign(TxHash).ToByteArray(StringEncoding.Base64).ToBase58Check();
            Signture = ec.Sign(TxHash);
            ScriptPubKey = $"{Signture};{TransactionIssuer}";
            ScriptSig = $"{ScriptPubKey};CheckSig;IsOne";
        }
        [JsonConstructor]
        public Transaction(string transactionIssuer, string reciepient,
            double amount, DateTime isuueTime, DateTime approvalTime,
            string txHash, string txMessage, Guid nodeID, string signture,
            string scriptPubKey, string scriptSig)
        {
            TransactionIssuer = transactionIssuer;
            Reciepient = reciepient;
            Amount = amount;
            IsuueTime = isuueTime;
            ApprovalTime = approvalTime;
            TxHash = txHash;
            TxMessage = txMessage;
            NodeID = nodeID;
            Signture = signture;
            ScriptPubKey = scriptPubKey;
            ScriptSig = scriptSig;
        }
        #endregion
        public bool ISSigntureVerified()
        {
            if (TxHash == ComputeObjectHash())
            {
                //var ecdsa = new ECDSA(TransactionIssuer);

                //return ecdsa.Verify(Signture.ToByteArray(StringEncoding.Base85Check).ToBase64(), TxHash);

                var machine = new PUnite();
                machine.Push(TxHash);
                machine.Parser(ScriptSig);
                return machine.Process() ?? false;
            }
            return false;
        }
        public string ComputeObjectHash()
            => $"{TransactionIssuer}-{Reciepient}-{Amount}-{IsuueTime}".ComputeHashString();

        public bool Equal(IPICObject obj)
            => this.ToJson().ComputeHashString().Equals(obj.ToJson().ComputeHashString());
    }
}
