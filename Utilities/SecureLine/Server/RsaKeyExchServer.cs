using ParsiCoin.Base.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ParsiCoin.Base.SecureLine.Server
{
    class RsaKeyExchServer : IDisposable
    {
        private readonly RSACryptoServiceProvider _rsa;

        #region ctor
        public RsaKeyExchServer(string pubKey)
        {
            _rsa = new RSACryptoServiceProvider();
            _rsa.ImportCspBlob(Convert.FromBase64String(pubKey));
        }
        #endregion

        public string ExportPassWord(AES Aes)
        {
            var EncryptedKey = _rsa.Encrypt(Aes.PassWord, false);
            return Convert.ToBase64String(EncryptedKey);
        }


        public void Dispose()
        {
            _rsa.Dispose();
        }
    }
}
