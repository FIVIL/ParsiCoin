using ParsiCoin.Base.SecureLine.Client;
using ParsiCoin.Base.SecureLine.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsiCoin.Base.SecureLine
{
    class Usage
    {
        public Usage()
        {
            var c = new SecureLineClient();
            var s = new SecureLineServer(c.PubKey);
            var password = s.InitaiteClient();
            c.InitaiteServer(password);
            while (true)
            {
                var data = Console.ReadLine();
                var h = c.Encrypt(data);
                Console.WriteLine(s.Decrypt(h));
                data = Console.ReadLine();
                var h2 = s.Encrypt(data);
                Console.WriteLine(c.Decrypt(h2));
            }
        }
    }
}
