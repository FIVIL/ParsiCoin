using ParsiCoin.Base.Crypto;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ParsiCoin.Base.SecureLine.Client
{
    class RsaKeyExchClient : IDisposable
    {
        private readonly RSACryptoServiceProvider _rsa;

        private readonly string _pubKey;

        public string PubKey { get => _pubKey; }
        #region ctor
        public RsaKeyExchClient()
        {
            _rsa = new RSACryptoServiceProvider();
            _pubKey = Convert.ToBase64String(_rsa.ExportCspBlob(false));
        }
        #endregion

        public AES ImportPassword(string Pass)
        {
            var key = Convert.FromBase64String(Pass);
            var K = new Guid(_rsa.Decrypt(key, false));
            return new AES(K);
        }



        public void Dispose()
        {
            _rsa.Dispose();
        }
    }
}
