using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin.Base
{
    public interface IPICObject
    {
        bool Equal(IPICObject obj);
        string ComputeObjectHash();
    }
}
