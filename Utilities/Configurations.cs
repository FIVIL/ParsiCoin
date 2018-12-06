using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin.Base
{
    public class Configurations : IPICObject
    {
        public string PrivateKey = string.Empty;
        public int diffratio = 3;
        public byte diffpoint = 0xff;
        public string Path { get; set; } = "Data\\";
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
