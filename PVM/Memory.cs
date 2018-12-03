using System;
using System.Collections.Generic;
using System.Text;
using ParsiCoin.Base.Utilities;
namespace ParsiCoin.PVM
{
    public class Memory
    {
        private readonly Dictionary<byte[], string> _data;
        public int Count { get => _data.Count; }
        public Memory()
        {
            _data = new Dictionary<byte[], string>();
        }
        public byte[] Add(string value)
        {
            var res = value.ComputeHash(HashAlgorithms.SHA512);
            _data.Add(res, value);
            return res;
        }
        public string this[byte[] index]
        {
            get => _data[index];
        }
        public bool Remove(byte[] key) => _data.Remove(key);
        public void Clear() => _data.Clear();
        public bool ContainsKey(byte[] key) => _data.ContainsKey(key);
        public bool ContainsValue(string value) => _data.ContainsValue(value);
    }
}
