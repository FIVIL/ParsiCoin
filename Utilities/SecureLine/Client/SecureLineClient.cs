using ParsiCoin.Base.Crypto;
using System;
using System.Collections.Generic;
using System.Text;
using ParsiCoin.Base.Utilities;

namespace ParsiCoin.Base.SecureLine.Client
{
    public class SecureLineClient
    {
        private AES _aes;
        private readonly RsaKeyExchClient _rsaKeyXchC;

        public SecureLineClient()
        {
            _rsaKeyXchC = new RsaKeyExchClient();
        }

        public string PubKey { get => _rsaKeyXchC.PubKey; }

        public void InitaiteServer(string send)
        {
            _aes = _rsaKeyXchC.ImportPassword(send);
        }

        public string Encrypt(string message)
        {
            if (_aes is null) throw new Exception("Not initaited");
            return _aes.Encrypt(message).ToBase64();
        }

        public string Decrypt(string message)
        {
            if (_aes is null) throw new Exception("Not initaited");
            return _aes.Decrypt(message).FromByteArray();
        }
    }
}
