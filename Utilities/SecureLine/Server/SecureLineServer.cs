using ParsiCoin.Base.Crypto;
using System;
using System.Collections.Generic;
using System.Text;
using ParsiCoin.Base.Utilities;
namespace ParsiCoin.Base.SecureLine.Server
{
    public class SecureLineServer:IDisposable
    {
        private AES _aes;
        private readonly RsaKeyExchServer _rsaKeyXchS;

        public SecureLineServer(string pubKey)
        {
            _rsaKeyXchS = new RsaKeyExchServer(pubKey);
            _aes = new AES();
        }

        public string InitaiteClient() => _rsaKeyXchS.ExportPassWord(_aes);

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

        public void Dispose()
        {
            ((IDisposable)_aes).Dispose();
        }
    }
}
