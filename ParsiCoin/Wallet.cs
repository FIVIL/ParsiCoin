using ParsiCoin.Base.Crypto;
using ParsiCoin.Base.Utilities;
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

        public int _primaryAcc;
        public Account PrimaryAccount { get => Accounts[_primaryAcc]; }
        public double Balance { get; set; }
        //peers
        public Wallet(List<KeyValuePair<string, Guid>> privateKeys)
        {
            Accounts = new List<Account>();
            _primaryAcc = Services.Conf.PrimaryAcc;
            foreach (var item in privateKeys)
            {
                Accounts.Add(new Account(item.Key, item.Value));
            }
            Balance = Accounts.Sum(x => x.Balance);
        }
        public Transaction TransactionBuilder(string reciepient, double value, string message = "")
            => PrimaryAccount.TransactionBuilder(reciepient, value, message);
        public string SignMessage(string message) => PrimaryAccount.SignMessage(message);
        public bool IsSignVerified(string message, string sign)
            => PrimaryAccount.IsSignVerified(message, sign);
        public void ChangeAccount(int i)
        {
            _primaryAcc = i;
        }
    }
}
