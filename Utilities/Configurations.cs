using ParsiCoin.Base.Crypto;
using ParsiCoin.Base.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin.Base
{
    public class Configurations : IPICObject
    {
        public List<string> PrivateKeys { get; set; }
        public int diffratio { get; set; }
        public byte diffpoint { get; set; }
        public string Path { get; set; }
        public Configurations(string privateKey)
        {
            PrivateKeys = new List<string>();
            PrivateKeys.Add(privateKey);
            diffratio = 3;
            diffpoint = 0xff;
            Path = "Data\\";
        }
        public void AddKey(string key)
        {
            PrivateKeys.Add(key);
            Update();
        }
        public void Update()
        {
            var c = this;
            var cc = c.ToJson();
            var aes = new AES(Util.PassWord);
            var cce = aes.Encrypt(cc.ToByteArray());
            System.IO.File.WriteAllBytes("Configurations.dat", cce);
        }
        public string ComputeObjectHash()
        {
            throw new NotImplementedException();
        }

        public bool Equal(IPICObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
