using ParsiCoin.Base.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ParsiCoin.Base.Crypto
{
    public class AES : IDisposable
    {
        private readonly byte[] _password;
        private readonly Aes _aes;
        private readonly byte[] _salt;
        private readonly int _iterationCount;
        public byte[] PassWord { get => _password; }
        public AES(string PassWord) : this(new object())
        {
            _password = Utilities.Util.ToByteArray(PassWord);

            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_password, _salt, _iterationCount);
            _aes.Key = pdb.GetBytes(32);
            _aes.IV = pdb.GetBytes(16);
            pdb.Dispose();
        }
        public AES(Guid PassWord) : this(new object())
        {
            _password = PassWord.ToByteArray();

            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_password, _salt, _iterationCount);
            _aes.Key = pdb.GetBytes(32);
            _aes.IV = pdb.GetBytes(16);
            pdb.Dispose();
        }
        public AES() : this(Guid.NewGuid())
        {
        }
        private AES(object obj)
        {
            _aes = Aes.Create();
            _salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
            _iterationCount = 20_000;
        }
        public byte[] Encrypt(byte[] clearBytes)
        {
            byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, _aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                res = ms.ToArray();
            }
            return res;
        }
        public byte[] Decrypt(byte[] cipherBytes)
        {
            byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, _aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                res = ms.ToArray();
            }
            return res;
        }

        public byte[] Encrypt(string clearText, StringEncoding encoding = StringEncoding.UTF8)
            => Encrypt(clearText.ToByteArray(encoding));
        public byte[] Decrypt(string cipherText)
            => Decrypt(cipherText.ToByteArray(StringEncoding.Base64));
        public void Dispose()
        {
            _aes.Dispose();
        }
    }
}
