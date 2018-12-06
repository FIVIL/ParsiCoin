using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin.Base
{
    public class Configurations : IPICObject
    {
        public string PrivateKey { get; set; }
        public int diffratio { get; set; }
        public byte diffpoint { get; set; }
        public string Path { get; set; }
        public Configurations(string privateKey)
        {
            PrivateKey = privateKey;
            diffratio = 3;
            diffpoint = 0xff;
            Path = "Data\\";
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
