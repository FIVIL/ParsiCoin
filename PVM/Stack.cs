using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin.PVM
{
    public class Stack
    {
        private readonly byte[][] _data;
        private int SP;
        public Stack()
        {
            _data = new byte[1024][];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = new byte[64];
            }
            SP = 0;
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
    }
}
