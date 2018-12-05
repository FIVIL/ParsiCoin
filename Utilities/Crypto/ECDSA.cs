//Based on NBitCoin https://github.com/MetacoSA/NBitcoin
using System;
using System.Collections.Generic;
using System.Text;
using NBitcoin;
using ParsiCoin.Base.Utilities;

namespace ParsiCoin.Base.Crypto
{
    public class ECDSA
    {
        private readonly Mnemonic _mnemonic;
        private readonly ExtKey _hdRoot;
        private readonly Key _privateKey;
        private readonly PubKey _pubKey;
        private readonly string[] _wordList;
        public string ExportPubKey { get => _pubKey.ToBytes().ToBase58Check(); }
        public string ExportPrivateKey
        {
            get =>
$"{_privateKey.ToBytes().ToBase58Check()} {_hdRoot.ChainCode.ToBase58Check()}".ToByteArray().ToBase58Check();
        }
        public string GetWords
        {
            get
            {
                var s = new StringBuilder();
                foreach (var item in _wordList)
                {
                    s.Append($"{item} ");
                }
                return s.Remove(s.Length - 1, 1).ToString();
            }
        }
        public bool OnlyPublic { get; set; }

        public ECDSA(string key, bool OnlyPublic = true)
        {
            if (OnlyPublic)
            {
                _mnemonic = null;
                _hdRoot = null;
                _privateKey = null;
                _pubKey = new PubKey(key.ToByteArray(StringEncoding.Base85Check));
                _wordList = null;
                OnlyPublic = true;
            }
            else
            {
                if (key.Split(' ').Length == 12)
                {
                    _mnemonic = new Mnemonic(key);
                    _hdRoot = _mnemonic.DeriveExtKey();
                    _privateKey = _hdRoot.PrivateKey;
                    _pubKey = _privateKey.PubKey;
                    _wordList = _mnemonic.Words;
                    OnlyPublic = false;
                }
                else
                {
                    var keys = key.ToByteArray(StringEncoding.Base85Check).FromByteArray().Split(' ');
                    _mnemonic = null;
                    _privateKey = new Key(keys[0].ToByteArray(StringEncoding.Base85Check));
                    _hdRoot = new ExtKey(_privateKey, keys[1].ToByteArray(StringEncoding.Base85Check));
                    _pubKey = _hdRoot.PrivateKey.PubKey;
                    _wordList = null;
                    OnlyPublic = false;
                }
            }
        }
        public ECDSA()
        {
            _mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
            _hdRoot = _mnemonic.DeriveExtKey();
            _privateKey = _hdRoot.PrivateKey;
            _pubKey = _privateKey.PubKey;
            _wordList = _mnemonic.Words;
            OnlyPublic = false;
        }
        public string Sign(string Message)
        {
            if (OnlyPublic) throw new Exception("Private Key neded.");
            return _privateKey.SignMessage(Message);
        }
        public bool Verify(string Signture, string Message)
            => _pubKey.VerifyMessage(Message, Signture);
    }
}
