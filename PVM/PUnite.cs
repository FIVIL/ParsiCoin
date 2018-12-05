using System;
using System.Collections.Generic;
using System.Text;
using ParsiCoin.Base.Crypto;
using ParsiCoin.Base.Utilities;
namespace ParsiCoin.PVM
{
    public class PUnite
    {
        private readonly byte[] _one;
        private readonly byte[] _zero;
        private readonly Dictionary<Commands, Func<bool?>> _actions;
        private readonly Stack _stack;

        private readonly List<Commands> Codes;
        public PUnite()
        {
            Codes = new List<Commands>();
            _one = new byte[64];
            for (int i = 0; i < 64; i++)
            {
                _one[i] = 1;
            }
            _zero = new byte[64];
            for (int i = 0; i < 64; i++)
            {
                _zero[i] = 0;
            }
            _stack = new Stack();
            _actions = new Dictionary<Commands, Func<bool?>>();
            _actions.Add(Commands.Zero, () =>
            {
                _stack.Push(_zero);
                return null;
            });
            _actions.Add(Commands.One, () =>
            {
                _stack.Push(_one);
                return null;
            });
            _actions.Add(Commands.MD5, () =>
            {
                _stack.Pop(out string data);
                var data2 = data.ComputeHash(HashAlgorithms.MD5);
                //var data3 = new byte[64];
                //for (int i = 0; i < data2.Length; i++)
                //{
                //    data3[i] = data2[i];
                //}
                _stack.Push(data2);
                return null;
            });
            _actions.Add(Commands.SHA256, () =>
            {
                _stack.Pop(out string data);
                var data2 = data.ComputeHash(HashAlgorithms.SHA256);
                //var data3 = new byte[64];
                //for (int i = 0; i < data2.Length; i++)
                //{
                //    data3[i] = data2[i];
                //}
                _stack.Push(data2);
                return null;
            });
            _actions.Add(Commands.DoubleSHA256, () =>
            {
                _stack.Pop(out string data);
                var data2 = data.ComputeHash(HashAlgorithms.DoubleSHA256);
                //var data3 = new byte[64];
                //for (int i = 0; i < data2.Length; i++)
                //{
                //    data3[i] = data2[i];
                //}
                _stack.Push(data2);
                return null;
            });
            _actions.Add(Commands.SHA512, () =>
            {
                _stack.Pop(out string data);
                var data2 = data.ComputeHash(HashAlgorithms.SHA512);
                _stack.Push(data2);
                return null;
            });
            _actions.Add(Commands.DoubleSHA512, () =>
            {
                _stack.Pop(out string data);
                var data2 = data.ComputeHash(HashAlgorithms.DoubleSHA512);
                _stack.Push(data2);
                return null;
            });

            _actions.Add(Commands.Dup, () =>
            {
                _stack.Pop(out string data);
                _stack.Push(data);
                return null;
            });
            _actions.Add(Commands.CheckSig, () =>
            {
                _stack.Pop(out string PubKey);
                _stack.Pop(out string Sig);
                _stack.Pop(out string Message);
                //var MessageHash = Message.ComputeHashString(HashAlgorithms.DoubleSHA512);
                var ecdsa = new ECDSA(PubKey);
                var res = ecdsa.Verify(Sig, Message);
                if (res) _stack.Push(_one);
                else _stack.Push(_zero);
                return null;
            });
            _actions.Add(Commands.IsOne, () =>
            {
                var data = _stack.Peek;
                var res = true;
                foreach (var item in data)
                {
                    if (item != 1)
                    {
                        res = false;
                        break;
                    }
                }
                return res;
            });
            _actions.Add(Commands.IsZero, () =>
            {
                var data = _stack.Peek;
                var res = true;
                foreach (var item in data)
                {
                    if (item != 0)
                    {
                        res = false;
                        break;
                    }
                }
                return res;
            });
            _actions.Add(Commands.Eq, () =>
            {
                _stack.Pop(out string data1);
                _stack.Pop(out string data2);
                var res = data1.Equals(data2);
                if (res) _stack.Push(_one);
                else _stack.Push(_zero);
                return null;
            });
        }
        public void AddCommand(Commands cm) => Codes.Add(cm);
        public bool? Process()
        {
            bool? res = null;
            foreach (var item in Codes)
            {
                try
                {
                    res = _actions[item].Invoke();
                }
                catch
                {
                    return false;
                }
            }
            return res;
        }
        public bool Push(string data) => _stack.Push(data);
        public void Parser(string s)
        {
            var ss = s.Split(';');
            foreach (var item in ss)
            {
                if (Enum.TryParse<Commands>(item, out var cm))
                {
                    AddCommand(cm);
                }
                else
                {
                    Push(item);
                }
            }
        }
    }
}
