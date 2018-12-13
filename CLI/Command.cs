using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin.CLI
{
    public enum CommandName
    {
        Init,//-
        //Start,//- ; -o
        Exite,//-
        //Save,//-
        Sync,//-N -F
        Account,//-a show all; -g <index/pkey> for get; -p <index/pkey> for set primary; -n for new account
        PrivateKey,//-e <index/pkey?> <path> export private key; -i <path> import privatekey; -m <mnemonic> import mnemonic 
        UpdatePassword,// <oldPass> <newPass>
        Send,// <from?if not provided use primary> <amount> <recipient>
        Recive,// <to?if not provided use primary>
        Peer,//-a show all; -n <ip> set new peer;-d <ip/index> delete existing peer;
        //Config,//show configs
        Help,//<commandName?> Help of specific command
        cls//Clear Screen
    }
    public class Command
    {
        public CommandName Name { get; set; }
        public char[] AvailableSwitches { get; set; }
        public Action<char, string[]> Action { get; set; }

        public string Help { get; set; }
        public Dictionary<string, string> Errs { get; set; }
        public Command()
        {
            Errs = new Dictionary<string, string>();
        }

    }
}
