using LiteDB;
using ParsiCoin.Base;
using ParsiCoin.Base.Crypto;
using ParsiCoin.Base.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ParsiCoin.DB
{
    public class LiteDBRepository
    {
        private readonly string _path;
        private const string accounts = "Accounts";
        private const string accountMerkle = "AccM";
        private const string nodes = "Nodes";

        public LiteDBRepository(AES aes)
        {
            var cf = File.ReadAllBytes("Configurations.dat");

            var cfdec = aes.Decrypt(cf).FromByteArray().FromJson<Configurations>();
            Util.Conf = cfdec;
            _path = cfdec.Path;
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
            using (var db = new LiteDatabase(PathCombine(accounts)))
            {
                if (!db.CollectionExists(accounts))
                {
                    var acc = db.GetCollection<Account>(accounts);
                    acc.EnsureIndex(x => x.ID, true);
                    acc.EnsureIndex(x => x.HashStirng, true);
                }
            }
            using (var db = new LiteDatabase(PathCombine(accountMerkle)))
            {
                if (!db.CollectionExists(accountMerkle))
                {
                    //var acc = db.GetCollection<Account>(accountMerkle);
                }
            }
            using (var db = new LiteDatabase(PathCombine(nodes)))
            {
                if (!db.CollectionExists(nodes))
                {
                    var nods = db.GetCollection<Node>(nodes);
                    nods.EnsureIndex(x => x.ID, true);
                    nods.EnsureIndex(x => x.NodeHash, true);
                }
            }
        }
        private string PathCombine(string name/*, params string[] Dirs*/)
        {
            var sb = new StringBuilder(_path);
            //foreach (var item in Dirs)
            //{
            //    sb.Append($"\\{item}");
            //}
            sb.Append(FileName(name));
            return sb.ToString();
        }
        private string FileName(string name) => $"{name}.db";


        public Node GetNode(Guid id)
        {
            Node res = null;
            using (var db = new LiteDatabase(PathCombine(nodes)))
            {
                res = db.GetCollection<Node>(nodes)
                    .FindOne(x => x.ID == id);
            }
            return res;
        }

        public Account GetAccount(Guid id)
        {
            Account res = null;
            using (var db = new LiteDatabase(PathCombine(accounts)))
            {
                res = db.GetCollection<Account>(accounts)
                    .FindOne(x => x.ID == id);
            }
            return res;
        }
        public void UpdateNode(Node n)
        {
            using (var db = new LiteDatabase(PathCombine(nodes)))
            {
                db.GetCollection<Node>(nodes)
                     .Update(n);
            }
        }
        public void UpdateAccount(Account n)
        {
            using (var db = new LiteDatabase(PathCombine(accounts)))
            {
                db.GetCollection<Account>(accounts)
                     .Update(n);
            }

        }
        public void AddNode(Node n)
        {
            using (var db = new LiteDatabase(PathCombine(nodes)))
            {
                db.GetCollection<Node>(nodes)
                     .Insert(n);
            }
        }
        public void AddAccount(Account n)
        {
            using (var db = new LiteDatabase(PathCombine(accounts)))
            {
                db.GetCollection<Account>(accounts)
                     .Insert(n);
            }
        }
        public void AddNodeRange(IList<Node> n)
        {
            using (var db = new LiteDatabase(PathCombine(nodes)))
            {
                db.GetCollection<Node>(nodes)
                     .Insert(n);
            }
        }
        public void AddAccountRange(IList<Account> n)
        {
            using (var db = new LiteDatabase(PathCombine(accounts)))
            {
                db.GetCollection<Account>(accounts)
                     .Insert(n);
            }
        }
    }
}
