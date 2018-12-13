using ParsiCoin.Base.Crypto;
using System;
using System.Collections.Generic;
using System.Text;
using static ParsiCoin.CLI.MyConsole;
using System.Linq;
using ParsiCoin.Base.Utilities;

namespace ParsiCoin.CLI
{
    public class CommandLine
    {
        public Dictionary<CommandName, Command> Commands { get; set; }
        public CommandLine()
        {
            Commands = new Dictionary<CommandName, Command>();
            Commands[CommandName.Init] = new Command()
            {
                Name = CommandName.Init,
                AvailableSwitches = new char[1] { '0' },
                Help = string.Empty,
                Action = (c, s) =>
                {
                    Init();
                },
            };

            Commands[CommandName.Exite] = new Command()
            {
                Name = CommandName.Exite,
                AvailableSwitches = new char[1] { '0' },
                Help = string.Empty,
                Action = (c, s) =>
                {
                    Services.Conf.Update();
                    //disconnect from peers
                    Environment.Exit(0);
                },
            };

            Commands[CommandName.Sync] = new Command()
            {
                Name = CommandName.Sync,
                AvailableSwitches = new char[2] { 'N', 'F' },
                Help = string.Empty,
                Action = (c, s) =>
                {
                    if (!(new char[2] { 'N', 'F' }).Contains(c))
                    {
                        throw new Exception($"This Command doesnt have switch {c}.");
                    }
                    //if (c == 'N') SyncOn();
                    //else SyncOff();
                },
            };

            Commands[CommandName.Account] = new Command()
            {
                Name = CommandName.Account,
                AvailableSwitches = new char[4] { 'A', 'G', 'P', 'N' },
                Help = @"-A : Show All Accounts. Usage: Account -A
-G <Specified account index?> : Get Specific Account Full Details. Usage: Account -G 1
-P <Specified account index?> : Set Specific Account As Primary Account. Usage: Account -P 1
-N : Create New Account. Usage: Account -N",
                Action = (c, s) =>
                {
                    if (!(new char[4] { 'A', 'G', 'P', 'N' }).Contains(c))
                    {
                        throw new Exception($"This Command doesnt have switch {c}.");
                    }
                    switch (c)
                    {
                        case 'A':
                            ShowAccounts();
                            break;
                        case 'G':
                            if (s.Length == 0) s = new string[] { "0" };
                            var acc = Services.Wallet.Accounts[int.Parse(s[0])];
                            WriteText($"Public key: {acc.GetPubKey}");
                            WriteText($"Balance: {acc.Balance}");
                            Console.WriteLine("Received Transactions:");
                            var index = 0;
                            Console.WriteLine("No. | Hash | Issuer | Amount | Confirmations");
                            foreach (var item in acc.InCome)
                            {
                                Console.WriteLine($"{++index} | {item.TxHash} | {item.IssuerPubKey} | {item.Tx.Amount} | {item.Confirmation}");
                            }
                            index = 0;
                            Console.WriteLine("Sent Transactions:");
                            Console.WriteLine("No. | Hash | Reciepient | Amount | Confirmations");
                            foreach (var item in acc.OutGo)
                            {
                                Console.WriteLine($"{++index} | {item.TxHash} | {item.Tx.Reciepient} | {item.Tx.Amount} | {item.Confirmation}");
                            }
                            break;
                        case 'P':
                            if (s.Length == 0) s = new string[] { "0" };
                            var sti = int.Parse(s[0]);
                            if (sti > Services.Wallet.AccCount) throw new Exception("No Account Available.");
                            Services.Conf.PrimaryAcc = sti - 1;
                            Services.Conf.Update();
                            Services.Wallet._primaryAcc = sti - 1;
                            break;
                        case 'N':
                            var ec = CreateAccount();
                            Services.Conf.AddKey(ec.ExportPrivateKey, Guid.NewGuid());
                            Services.InitFile(Util.PassWord);
                            break;
                        default:
                            ShowAccounts();
                            break;
                    }
                },
            };

            Commands[CommandName.PrivateKey] = new Command()
            {
                Name = CommandName.PrivateKey,
                AvailableSwitches = new char[3] { 'E', 'I', 'M' },
                Help = "-E <file path> : Export Primary Account Private Key As A File And Save File At The Specified Path. Usage: PrivateKey -E C:User\\[X]\\Desktop\n" +
                "-I <file path> : Import Private Key From A File At Specified Path. Usage: PrivateKey -I C:User\\[X]\\Desktop\n" +
                "-M <Mnemonic> : Import Private Key From A Mnemonic Passphrase. Usage: PrivateKey -M [1] [2] [3] ...\n",
                Action = (c, s) =>
                {
                    if (!(new char[3] { 'E', 'I', 'M' }).Contains(c))
                    {
                        throw new Exception($"This Command doesnt have switch {c}.");
                    }
                    switch (c)
                    {
                        case 'E':
                            if (s.Length < 1 || !System.IO.Directory.Exists(s[0]))
                            {
                                throw new Exception("Wrong File Path.");
                            }
                            WriteText("Enter a password for key file:", false);
                            var pass = ReadPass();
                            var key = Newtonsoft.Json.JsonConvert.SerializeObject(Services.Conf.PrivateKeys[Services.Conf.PrimaryAcc]);
                            var aes = new AES(pass);
                            var enckey = aes.Encrypt(key);
                            System.IO.File.WriteAllBytes($"{s[0]}\\key.PrivateKey", enckey);
                            break;
                        case 'I':
                            if (s.Length < 1 || !System.IO.File.Exists(s[0]))
                            {
                                throw new Exception("No such file existe.");
                            }
                            WriteText("Enter a password for key file:", false);
                            pass = ReadPass();
                            aes = new AES(pass);
                            var k = System.IO.File.ReadAllBytes(s[0]);
                            try
                            {
                                var deck = aes.Decrypt(k).FromByteArray();
                                var key2 = Newtonsoft.Json.JsonConvert.DeserializeObject<KeyValuePair<string, Guid>>(deck);
                                if (!Services.Conf.PrivateKeys.Exists(x => x.Value == key2.Value))
                                {
                                    Services.Conf.AddKey(key2.Key, key2.Value);
                                    Services.InitFile(Util.PassWord);
                                }
                            }
                            catch { throw new Exception("Wrong Password."); }
                            break;
                        case 'M':
                            throw new NotImplementedException("Not yet available.");
                            break;
                        default:
                            break;
                    }
                },
            };

            Commands[CommandName.UpdatePassword] = new Command()
            {
                Name = CommandName.UpdatePassword,
                AvailableSwitches = new char[1] { '0' },
                Help = "Updates the password",
                Action = (c, s) =>
                {
                    throw new NotImplementedException("Not yet available.");
                },
            };

            Commands[CommandName.Send] = new Command()
            {
                Name = CommandName.Send,
                AvailableSwitches = new char[1] { '0' },
                Help = "<from?> <amount> <to(PubKey)> : Sends specified funds from selected account to pubkey address. Usage: Send 1 0.25 [pubkey]",
                Action = (c, s) =>
                {
                    if (s.Length == 2)
                    {
                        var tx = Services.Wallet.TransactionBuilder(s[1], double.Parse(s[0]));
                        Console.WriteLine(tx.ToJson(true));
                    }
                    else if (s.Length == 3)
                    {
                        var tx = Services.Wallet.Accounts[int.Parse(s[0])]
                                    .TransactionBuilder(s[2], double.Parse(s[1]));
                        Console.WriteLine(tx.ToJson(true));
                    }
                    else throw new Exception("You should provide input for this command.");
                },
            };

            Commands[CommandName.Recive] = new Command()
            {
                Name = CommandName.Recive,
                AvailableSwitches = new char[1] { '0' },
                Help = "<from?> : Show account public key to recive. Usage: Recive 1",
                Action = (c, s) =>
                {
                    if (s.Length == 0) WritePrimary(Services.Wallet.PrimaryAccount.GetPubKey);
                    else if (s.Length == 1) WritePrimary(Services.Wallet.Accounts[int.Parse(s[0])].GetPubKey);
                    else throw new Exception("You should provide input for this command.");
                },
            };

            Commands[CommandName.Peer] = new Command()
            {
                Name = CommandName.Peer,
                AvailableSwitches = new char[3] { 'A', 'N', 'D' },
                Help = @"-A : Show all connected peers. Usage: Peer -A
-N <peerIP> : Set new Peer. Usage: Peer -N [0.0.0.0]
-D <peerID> : Delete specified peer from connected peers. Usage: Peer -D 0",
                Action = (c, s) =>
                {
                    throw new NotImplementedException("Not yet available.");
                },
            };


            Commands[CommandName.Help] = new Command()
            {
                Name = CommandName.Help,
                AvailableSwitches = new char[1] { '0' },
                Help = @" : Show Helps. Usage: Help
<CommandName> : Show help for specific command. Usage: Help Send",
                Action = (c, s) =>
                {
                    if (s.Length == 0)
                    {
                        var hs = new StringBuilder();
                        foreach (var item in Enum.GetValues(typeof(CommandName)))
                        {
                            hs.AppendLine(item.ToString());
                        }
                        Console.WriteLine(hs.ToString());
                    }
                    else
                    {
                        if (!Enum.TryParse<CommandName>(s[0], true, out var cm))
                        {
                            throw new Exception("No such command were found.");
                        }
                        Console.WriteLine(Commands[cm].Help);
                    }
                },
            };

            Commands[CommandName.cls] = new Command()
            {
                Name = CommandName.cls,
                AvailableSwitches = new char[1] { '0' },
                Help = @"Clears The Screen",
                Action = (c, s) =>
                {
                    Console.Clear();
                },
            };

        }
        public void Parser()
        {
            Commands[CommandName.Init].Action('0', new string[] { });
            do
            {
                var cs = Read();
                var cms = cs.Split(' ');
                if (cms.Length > 0)
                {
                    if (!Enum.TryParse<CommandName>(cms[0], false, out var cm))
                    {
                        WriteErr("Not such command were found.");
                    }
                    else
                    {
                        if (cms.Length > 1)
                        {
                            if (cms[1].Length == 2 && cms[1].Contains('-'))
                            {

                                string[] inps = new string[] { };
                                if (cms.Length > 2)
                                {
                                    inps = new string[cms.Length - 2];
                                    for (int i = 2; i < cms.Length; i++)
                                    {
                                        inps[i - 2] = cms[i];
                                    }
                                }
                                try
                                {
                                    Commands[cm].Action.Invoke(cms[1][1], inps);
                                }
                                catch (Exception ex)
                                {
                                    WriteErr(ex.Message);
                                    WritePrimary($"You can see this command help using:Help {cm}");
                                }
                            }
                            else
                            {
                                var inps = new string[cms.Length - 1];
                                for (int i = 1; i < cms.Length; i++)
                                {
                                    inps[i - 1] = cms[i];
                                }
                                try
                                {
                                    Commands[cm].Action.Invoke('0', inps);
                                }
                                catch (Exception ex)
                                {
                                    WriteErr(ex.Message);
                                    WritePrimary($"You can see this command help using:Help {cm}");
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                Commands[cm].Action.Invoke('0', new string[] { });
                            }
                            catch (Exception ex)
                            {
                                WriteErr(ex.Message);
                                WritePrimary($"You can see this command help using:Help {cm}");
                            }
                        }
                    }
                }
            } while (true);
        }
        private void Init()
        {
            WritePrimary("Parsi Coin [Version 1.0]");
            WriteText("Source Available On ", false);
            WriteSuccess("GitHub: ", false);
            WriteDanger("https://github.com/FIVIL/ParsiCoin", false);
            EndLine();
            if (System.IO.File.Exists("Configurations.dat"))
            {
                do
                {
                    Console.WriteLine("Enter Your Password: ");
                } while (!OpenConfig(ReadPass()));
            }
            else
            {
                WriteText("Preaparing system for first use...");
                WriteText("Please choose a password:", false);
                var pass = ReadPass();

                WriteSuccess("Please wait untill we finish starting up...");
                var ec = CreateAccount();
                Services.FirstInit(pass, ec);
            }
            WritePrimary("Checking System Status.");
            CheckNetwork();
            Sync();
            EndLine();
            WriteText("Primary Account:", false);
            WritePrimary($"{Services.Wallet.PrimaryAccount.GetPubKey}", false);
            WriteText($"Balance: {Services.Wallet.PrimaryAccount.Balance} PIC");
            WriteText("Totall Balance:", false);
            WriteText($"{Services.Wallet.Balance} PIC");
        }
        private ECDSA CreateAccount()
        {
            var ec = new ECDSA();
            WriteText("Your mnemonic passphrase is:", false);
            WriteDanger("(remember to store this in a safe space)");
            WriteText(ec.GetWords);
            WriteText("Press any key to continue.");
            Console.ReadKey();
            CLS();
            var ws = ec.GetWords.Split(' ');
            for (int i = 0; i < 4; i++)
            {
                var index = Base.Utilities.Util.Rnd.Next(1, 13);
                WriteText($"Enter the {index}th word:", false);
                var r = Console.ReadLine();
                while (r != ws[index - 1])
                {
                    WriteErr("Wrong!!!");
                    WriteText($"Enter the {index}th word:", false);
                    r = Console.ReadLine();
                }
            }
            CLS();
            return ec;
        }
        private void CheckNetwork()
        {
            WriteText("NetWork: ", false);
            WriteErr("Offline", false);
            EndLine();
        }
        private void Sync()
        {
            WriteText("Tangle: ", false);
            WriteDanger("Syncing", false);
            EndLine();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Date Recived.");
            }
        }
        private void ShowAccounts()
        {
            WriteText("Accounts:");
            WriteText("Public key                                         | Balance");
            foreach (var item in Services.Wallet.Accounts)
            {
                Console.WriteLine($"{item.GetPubKey} | {item.Balance}");
            }
        }
        private bool OpenConfig(string pass)
        {
            try
            {
                Services.InitFile(pass);
            }
            catch (Exception)
            {
                WriteErr("Wrong password");
                return false;
            }
            return true;
        }
        private string Read()
        {
            var s = Services.Wallet.PrimaryAccount.GetPubKey.ToString().Substring(0, 10);
            WriteText($"PC: [{DateTime.Now}] |", false);
            WriteText($"{s}... |", false);
            WriteSuccess("Synced", false);
            WriteText(">", false);
            return Console.ReadLine();
        }
    }
}
