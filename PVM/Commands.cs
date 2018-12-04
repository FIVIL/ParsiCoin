using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin.PVM
{
    public enum Commands
    {
        Zero,
        One,
        //
        MD5,
        SHA256,
        DoubleSHA256,
        SHA512,
        DoubleSHA512,
        //
        Dup,
        CheckSig,
        //
        IsOne,
        IsZero,
        //
        Eq
    }
}
