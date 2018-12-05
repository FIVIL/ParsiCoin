﻿using Newtonsoft.Json;
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
        public DateTime PublishTime { get; set; }

        public Guid Left { get; private set; }
        public Guid Right { get; private set; }
        public string LeftHash { get; private set; }
        public string RightHash { get; private set; }

        [StringLength(100)]
        public string Message { get; private set; }

        public Transaction Tx { get; private set; }

        public string TxHash { get; private set; }

        public string IssuerPubKey { get; private set; }
        public string NodeHash { get; private set; }

        #region ctor
        public Node(Node left, Node right, string message, string Issuer, Transaction tx = null)
        {
            if (!(tx is null) && tx.TransactionIssuer != Issuer) throw new Exception();
            ID = Guid.NewGuid();
            MintTime = DateTime.Now;
            Left = left.ID;
            LeftHash = left.NodeHash;
            Right = right.ID;
            RightHash = right.NodeHash;
            Message = message;
            Tx = tx;
            TxHash = tx?.TxHash;
            IssuerPubKey = Issuer;
            NodeHash = ComputeTxHash();
        }
        public Node()
        {
            ID = Guid.NewGuid();
        }
        [JsonConstructor]
        public Node(Guid iD, Guid left, Guid right, string message, string issuerPubKey, Transaction tx, string txHash
            , DateTime mintTime, DateTime publishTime, string leftHash, string rightHash, string nodeHash)
        {
            ID = iD;
            MintTime = mintTime;
            Left = left;
            LeftHash = leftHash;
            Right = right;
            RightHash = rightHash;
            Message = message;
            Tx = tx;
            TxHash = txHash;
            IssuerPubKey = issuerPubKey;
            NodeHash = nodeHash;
            PublishTime = publishTime;
        }
        public string ComputeTxHash()
            => $"{ID}-{MintTime}-{LeftHash}-{RightHash}-{Message.ComputeHashString()}-{TxHash}-{IssuerPubKey}".ComputeHashString();

        #endregion
        public bool Equal(IPICObject obj)
            => this.ToJson().ComputeHashString().Equals(obj.ToJson().ComputeHashString());
    }
}
