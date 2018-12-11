using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ParsiCoin.Base.Utilities;
using ParsiCoin.Base.Crypto;
using ParsiCoin.Base;

namespace ParsiCoin.DB
{
    [Obsolete("Never Used,Substitute with liteDB")]
    public class Context
    {
        private readonly string _path;
        private const string accounts = "Accounts";
        private const string accountMerkle = "AccM";
        private const string nodes = "Nodes";
        private const string config = "Configurations";
        //private const string settings = "Settings";
        private const string lib = "Paths";

        private Context()
        {
            var cf = File.ReadAllBytes(PathCombine(config, 0, "config"));
            using (var aes = new AES(Util.PassWord))
            {
                var cfdec = aes.Decrypt(cf).FromByteArray().FromJson<Configurations>();
                Util.Conf = cfdec;
                _path = cfdec.Path;
            }
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
                Directory.CreateDirectory($"{_path}\\{lib}");
                Directory.CreateDirectory($"{_path}\\{accounts}");
                Directory.CreateDirectory($"{_path}\\{nodes}");
                Directory.CreateDirectory($"{_path}\\{accountMerkle}");
            }
            else
            {
                if (!Directory.Exists($"{_path}\\{lib}"))
                    Directory.CreateDirectory($"{_path}\\{lib}");

                if (!Directory.Exists($"{_path}\\{accounts}"))
                    Directory.CreateDirectory($"{_path}\\{accounts}");

                if (!Directory.Exists($"{_path}\\{nodes}"))
                    Directory.CreateDirectory($"{_path}\\{nodes}");

                if (!Directory.Exists($"{_path}\\{accountMerkle}"))
                    Directory.CreateDirectory($"{_path}\\{accountMerkle}");
            }

        }
        private string PathCombine(string name, int c, params string[] Dirs)
        {
            var sb = new StringBuilder(_path);
            foreach (var item in Dirs)
            {
                sb.Append($"\\{item}");
            }
            sb.Append(FileName(name, c));
            return sb.ToString();
        }
        private string FileName(string name, int c) => $"{name}_{c}.dat";

        private Dictionary<Guid, string> AccAddreses { get; set; }
        private Dictionary<Guid, string> NodeAddreses { get; set; }
        private Dictionary<Guid, Account> Accounts { get; set; }
        private Dictionary<Guid, Node> Nodes { get; set; }

        private void Add()
        {

        }
        private void Update()
        {

        }

        public Node LoadNode(Guid id)
        {
            throw new NotImplementedException();
        }

        public Account LoadAccount(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
