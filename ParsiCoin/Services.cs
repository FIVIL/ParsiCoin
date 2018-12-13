using ParsiCoin.Base;
using System;
using System.Collections.Generic;
using System.Text;
using ParsiCoin.Base.Utilities;
using ParsiCoin.Base.Crypto;
using ParsiCoin.DB;

namespace ParsiCoin
{
    public static class Services
    {
        public const string config = "Configurations";
        public static LiteDBRepository db { get; set; } = null;
        public static AES aes { get; set; } = null;
        public static Wallet Wallet { get; set; } = null;
        public static List<Transaction> TransactionPool { get; set; }
        public static Configurations Conf { get => Util.Conf; }
        //wallet
        public static void InitFile(string password)
        {
            Util.PassWord = password;
            TransactionPool = new List<Transaction>();

            aes = new AES(Util.PassWord);
            db = new LiteDBRepository(aes);
            Wallet = new Wallet(Util.Conf.PrivateKeys);
        }
        public static void FirstInit(string password, ECDSA ecdsa)
        {
            Util.PassWord = password;
            TransactionPool = new List<Transaction>();

            var id = Guid.NewGuid();
            //
            var c = new Configurations(new KeyValuePair<string, Guid>(ecdsa.ExportPrivateKey, id));
            var cc = c.ToJson();
            aes = new AES(Util.PassWord);
            var cce = aes.Encrypt(cc.ToByteArray());
            System.IO.File.WriteAllBytes("Configurations.dat", cce);
            db = new DB.LiteDBRepository(aes);
            new MerkleTree();
            Wallet = new Wallet(new List<KeyValuePair<string, Guid>>() { new KeyValuePair<string, Guid>(ecdsa.ExportPrivateKey, id) });
        }
    }
}
