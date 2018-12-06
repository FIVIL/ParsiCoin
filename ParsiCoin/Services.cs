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
        //wallet
        public static void Init(string password)
        {
            Util.PassWord = password;
            if (System.IO.File.Exists("Configurations.dat"))
            {
                aes = new AES(Util.PassWord);
                db = new LiteDBRepository(aes);
                Wallet = new Wallet(Util.Conf.PrivateKey);
            }
            else
            {
                var ecdsa = new ECDSA();
                Console.WriteLine(ecdsa.GetWords);
                var c = new Configurations(ecdsa.ExportPrivateKey);
                var cc = c.ToJson();
                aes = new AES(Util.PassWord);
                var cce = aes.Encrypt(cc.ToByteArray());
                System.IO.File.WriteAllBytes("Configurations.dat", cce);
                db = new DB.LiteDBRepository(aes);
                Wallet = new Wallet(ecdsa.ExportPrivateKey);
            }
        }
    }
}
