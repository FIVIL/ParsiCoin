using ParsiCoin.Base.Crypto;
using ParsiCoin.Base.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin.Base
{
    public class Configurations : IPICObject
    {
        public List<KeyValuePair<string, Guid>> PrivateKeys { get; set; }
        public int diffratio { get; set; }
        public byte diffpoint { get; set; }
        public string Path { get; set; }

        public int PrimaryAcc { get; set; }
        public Configurations(KeyValuePair<string, Guid> privateKey)
        {
            PrivateKeys = new List<KeyValuePair<string, Guid>>();
            PrivateKeys.Add(privateKey);
            diffratio = 3;
            diffpoint = 0xff;
            Path = "Data\\";
            PrimaryAcc = 0;
        }
        public void AddKey(string key, Guid id)
        {
            PrivateKeys.Add(new KeyValuePair<string, Guid>(key, id));
            Update();
        }
        public void Update()
        {
            var c = this;
            var cc = c.ToJson();
            using (var aes = new AES(Util.PassWord))
            {
                var cce = aes.Encrypt(cc.ToByteArray());
                System.IO.File.WriteAllBytes("Configurations.dat", cce);
            }
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
