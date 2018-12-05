using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin.DB
{
    public class Context
    {
        private readonly string _path;
        private const string accounts = "Accounts";
        private const string accountMerkle = "AccM";
        private const string nodes = "Nodes";
        private const string config = "Configuration";
        private const string settings = "Settings";
        private const string lib = "Paths";

        private string PathCombine(string name, int c, params string[] Dirs)
        {
            var sb = new StringBuilder(_path);
            foreach (var item in Dirs)
            {
                sb.Append($"/{item}");
            }
            sb.Append(FileName(name, c));
            return sb.ToString();
        }
        private string FileName(string name, int c) => $"{name}_{c}.dat";

        public Dictionary<Guid, Account> Accounts { get; set; }
        public Dictionary<Guid, Node> Nodes { get; set; }

        private void Add()
        {

        }
    }
}
