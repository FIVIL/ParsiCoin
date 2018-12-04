using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin.PVM
{
    public enum Commands
    {
        Nop,
        //
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
        Sign,
        //
        IsOne,
        IsZero,
        //
        Eq
    }
}
