using ParsiCoin.Base.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParsiCoin
{
    public class Wallet
    {
        public List<Account> Accounts { get; set; }
        public int AccCount { get => Accounts.Count; }

        private int _primaryAcc;
        public Account PrimaryAccount { get => Accounts[_primaryAcc]; }
        public double Balance { get; set; }
        //peers
        public Wallet(List<string> privateKeys)
        {
            Accounts = new List<Account>();
            _primaryAcc = 0;
            foreach (var item in privateKeys)
            {
                Accounts.Add(new Account(item));
            }
            Balance = Accounts.Sum(x => x.Balance);
        }
    }
}
