using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ParsiCoin.Base.Utilities
{
    public static class Util
    {
        #region Props
        public static Random Rnd { get; set; } = new Random();
        private static byte[] _diff = null;
        public static Configurations Conf = null;
        public static string PassWord = string.Empty;
        public static byte[] Difficulty
        {
            get
            {
                if (_diff is null)
                {
                    var s = new byte[64];
                    for (int i = 0; i < Conf.diffratio; i++)
                    {
                        s[i] = 0;
                    }
                    s[Conf.diffratio] = Conf.diffpoint;
                    for (int i = Conf.diffratio + 1; i < 64; i++)
                    {
                        s[i] = 0xff;
                    }
                    _diff = s;
                }
                return _diff;
            }
        }
        #endregion

        #region StringEncoding
        public static string ToJson(this IPICObject obj, bool indented = false)
            => indented
            ? Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented)
            : Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None);

        public static T FromJson<T>(this string json) where T : IPICObject
            => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);

        public static byte[] ToByteArray(this string s, StringEncoding encoding = StringEncoding.UTF8)
        {
            switch (encoding)
            {
                case StringEncoding.Base64:
                    return Convert.FromBase64String(s);
                case StringEncoding.Base85Check:
                    return Base58Check.Base58Check.GetBytes(s);
                case StringEncoding.UTF8:
                    return System.Text.Encoding.UTF8.GetBytes(s);
                case StringEncoding.ASCII:
                    return System.Text.Encoding.ASCII.GetBytes(s);
                default:
                    return System.Text.Encoding.UTF8.GetBytes(s);
            }
        }

        public static string FromByteArray(this byte[] data, StringEncoding encoding = StringEncoding.UTF8)
        {
            switch (encoding)
            {
                case StringEncoding.Base64:
                    return Convert.ToBase64String(data);
                case StringEncoding.Base85Check:
                    return Base58Check.Base58Check.GetString(data);
                case StringEncoding.UTF8:
                    return System.Text.Encoding.UTF8.GetString(data);
                case StringEncoding.ASCII:
                    return System.Text.Encoding.ASCII.GetString(data);
                default:
                    return System.Text.Encoding.UTF8.GetString(data);
            }
        }

        public static string ToBase58Check(this byte[] data) => FromByteArray(data, StringEncoding.Base85Check);
        public static string ToBase64(this byte[] data) => FromByteArray(data, StringEncoding.Base64);
        #endregion

        #region Hash
        public static byte[] ComputeHash(this byte[] data, HashAlgorithms algorithm = HashAlgorithms.DoubleSHA512)
        {
            byte[] res = null;
            switch (algorithm)
            {
                case HashAlgorithms.MD5:
                    using (var hash = MD5.Create())
                    {
                        res = hash.ComputeHash(data);
                    }
                    break;
                case HashAlgorithms.SHA256:
                    using (var hash = SHA256.Create())
                    {
                        res = hash.ComputeHash(data);
                    }
                    break;
                case HashAlgorithms.DoubleSHA256:
                    using (var hash = SHA256.Create())
                    {
                        res = hash.ComputeHash(hash.ComputeHash(data));
                    }
                    break;
                case HashAlgorithms.SHA512:
                    using (var hash = SHA512.Create())
                    {
                        res = hash.ComputeHash(data);
                    }
                    break;
                case HashAlgorithms.DoubleSHA512:
                    using (var hash = SHA512.Create())
                    {
                        res = hash.ComputeHash(hash.ComputeHash(data));
                    }
                    break;
                default:
                    res = new byte[1];
                    break;
            }
            return res;
        }

        public static byte[] ComputeHash(this string s, HashAlgorithms algorithm = HashAlgorithms.DoubleSHA512)
            => ComputeHash(ToByteArray(s), algorithm);
        public static string ComputeHashString(this string s, HashAlgorithms algorithm = HashAlgorithms.DoubleSHA512)
            => FromByteArray(ComputeHash(ToByteArray(s), algorithm), StringEncoding.Base85Check);
        #endregion

        public static bool CompareDiff(this byte[] targer)
        {
            for (int i = 0; i < targer.Length; i++)
            {
                if (targer[i] > Difficulty[i]) return false;
            }
            return true;
        }

    }
}
