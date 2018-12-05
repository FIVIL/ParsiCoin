using System;
using System.Collections.Generic;
using System.Text;
using ParsiCoin.Base.Utilities;

namespace ParsiCoin.PVM
{
    public class Stack
    {
        private readonly byte[][] _data;
        private readonly Memory _mem;
        private int SP;
        public Stack()
        {
            _data = new byte[1024][];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = new byte[64];
            }
            _mem = new Memory();
            SP = -1;
        }
        public int SetSp(int value)
        {
            var temp = SP;
            SP = value;
            return temp;
        }
        public byte[] Peek { get => _data[SP]; }
        public bool Push(byte[] r)
        {
            if (SP < -1) throw new ArgumentException();
            SP++;
            _data[SP] = r;
            return true;
        }
        public bool Push(string s)
        {
            var r = s.ToByteArray();
            if (r.Length > 64)
            {
                return Push(_mem.Add(s));
            }
            else
            {
                return Push(r);
            }
        }
        public bool Pop(out byte[] r)
        {
            if (SP < 0)
            {
                r = null;
                return false;
            }
            r = _data[SP];
            SP--;
            return true;
        }
        public bool Pop(out string s)
        {
            var b = Pop(out byte[] r);
            if (!b)
            {
                s = string.Empty;
                return b;
            }
            if (_mem.ContainsKey(r))
            {
                s = _mem[r];
                _mem.Remove(r);
            }
            else
            {
                s = r.FromByteArray();
            }
            return true;
        }
        //private byte[] padding(byte[] inp)
        //{
        //    if (inp.Length < 64)
        //    {
        //        var res = new byte[64];
        //        for (int i = 0; i < inp.Length; i++)
        //        {
        //            res[i] = inp[i];
        //        }
        //        return res;
        //    }
        //    return inp;
        //}
    }
}
