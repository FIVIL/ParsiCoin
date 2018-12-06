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
        public static LiteDBRepository db { get; set; } = null;
        public static AES aes { get; set; } = null;
        //wallet
        public static void Init(string password)
        {
            Util.PassWord = password;
            if (System.IO.File.Exists("Configurations.dat"))
            {
                aes = new AES(Util.PassWord);
                db = new LiteDBRepository(aes);
            }
            else
            {
                var c = new Configurations();
                var cc = c.ToJson();
                aes = new AES(Util.PassWord);
                var cce = aes.Encrypt(cc.ToByteArray());
                System.IO.File.WriteAllBytes("Configurations.dat", cce);
                db = new DB.LiteDBRepository(aes);
            }
        }
    }
}
