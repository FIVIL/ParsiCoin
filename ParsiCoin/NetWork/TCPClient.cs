using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParsiCoin.NetWork
{
    public delegate void MessageReciveClient(string s);
    public class TCPClient
    {
        private Socket SClient;
        private IPEndPoint IP;
        public event MessageReciveClient OnMessageReciveClient;
        public int Port { get => IP.Port; }
        public TCPClient(int port, string ip = "127.0.0.1")
        {
            Init(port, ip);
        }
        private void Init(int port, string ip)
        {
            SClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IP = new IPEndPoint(IPAddress.Parse(ip), port);
            SClient.Connect(IP);
            Task.Run(() =>
            {
                while (true)
                {
                    byte[] barray = new byte[1024];
                    int RecB = SClient.Receive(barray);
                    if (RecB > 0)
                    {
                        string mess = Encoding.Unicode.GetString(barray, 0, RecB);
                        OnMessageReciveClient?.Invoke(mess);
                    }
                    Thread.Sleep(1);
                }
            });
        }
        public void Send(string s)
        {
            byte[] b = new byte[1024];
            b = Encoding.Unicode.GetBytes(s);
            Send(b);
        }
        private void Send(byte[] inbyte)
        {
            SClient.Send(inbyte);
        }
        public void Disconnect()
        {
            SClient?.Shutdown(SocketShutdown.Both);
            SClient?.Close();
            SClient?.Dispose();
            SClient = null;
        }
    }
}
