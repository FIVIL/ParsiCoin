using ParsiCoin;
using ParsiCoin.Base.Utilities;
using ParsiCoin.NetWork;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static ParsiCoin.CLI.MyConsole;
namespace ParsiCoin.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Parsi Coin";
            //Console.WriteLine();
            //var s = "    ____  ___    ____  _____ ____   __________  _____   __";
            //Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
            //Console.WriteLine(@"    ____  ___    ____  _____ ____   __________  _____   __");
            //Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
            //Console.WriteLine(@"   / __ \/   |  / __ \/ ___//  _/  / ____/ __ \/  _/ | / /");
            //Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
            //Console.WriteLine(@"  / /_/ / /| | / /_/ /\__ \ / /   / /   / / / // //  |/ / ");
            //Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
            //Console.WriteLine(@" / ____/ ___ |/ _, _/___/ // /   / /___/ /_/ // // /|  /  ");
            //Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
            //Console.WriteLine(@"/_/   /_/  |_/_/ |_|/____/___/   \____/\____/___/_/ |_/  ");
            //var l = Console.WindowWidth;
            //StringBuilder sb = new StringBuilder("  ");
            //for (int i = 0; i < l - 8; i++)
            //{
            //    sb.Append('_');
            //}
            //Console.WriteLine(sb.ToString());
            //Console.WriteLine();
            //Console.WriteLine();
            var cm = new CommandLine();
            cm.Parser();
            //var s = new TCPServer(8080);
            //s.OnMessageRecive += (sender, m) =>
            //{
            //    WriteSuccess(m);
            //};
            //var c = new TCPClient(8080, "127.0.0.1");
            //Console.WriteLine("OK");
            //while (true)
            //{
            //    var m = Console.ReadLine();
            //    c.Send(m);
            //}
            Console.ReadKey();
        }
    }
    public static class MyConsole
    {
        public static void WriteText(string txt, bool newLine = true)
        {
            if (newLine) Console.WriteLine(txt); else Console.Write($"{txt} ");
        }
        public static void WriteErr(string txt, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (newLine) Console.WriteLine(txt); else Console.Write($"{txt} ");
            Console.ResetColor();
        }
        public static void WriteDanger(string txt, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (newLine) Console.WriteLine(txt); else Console.Write($"{txt} ");
            Console.ResetColor();
        }
        public static void WriteSuccess(string txt, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (newLine) Console.WriteLine(txt); else Console.Write($"{txt} ");
            Console.ResetColor();
        }
        public static void WritePrimary(string txt, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (newLine) Console.WriteLine(txt); else Console.Write($"{txt} ");
            Console.ResetColor();
        }
        public static void EndLine() => Console.WriteLine();

        public static string ReadPass()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            EndLine();
            return pass;
        }
        public static void CLS() => Console.Clear();
    }

}
