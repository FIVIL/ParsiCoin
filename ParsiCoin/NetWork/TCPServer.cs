using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParsiCoin.NetWork
{
    public delegate void MessageRecive(object Sender, string s);
    public class TCPServer
    {
        private Socket SocServer;
        private Dictionary<IPEndPoint, Socket> SocClients;
        private IPEndPoint IP;
        public event MessageRecive OnMessageRecive;
        public int Port { get => IP.Port; }
        public TCPServer(int port)
        {
            Init(port);
        }
        private void Init(int port)
        {
            SocClients = new Dictionary<IPEndPoint, Socket>();
            IP = new IPEndPoint(IPAddress.Any, port);
            SocServer = new Socket(IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            SocServer.SetIPProtectionLevel(IPProtectionLevel.Unrestricted);
            Connect();
        }
        private void Connect()
        {
            Task.Run(() =>
            {
                SocServer.Bind(IP);
                SocServer.Listen(4);
                while (true)
                {
                    var ClientSoc = SocServer.Accept();
                    SocClients.Add(ClientSoc.RemoteEndPoint as IPEndPoint, ClientSoc);
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            byte[] barray = new byte[1024];
                            int RecB = ClientSoc.Receive(barray);
                            if (RecB > 0)
                            {
                                string mess = Encoding.Unicode.GetString(barray, 0, RecB);
                                OnMessageRecive?.Invoke(ClientSoc, mess);
                            }
                            Thread.Sleep(1);
                        }
                    });
                }
            });
        }
        public void SendAll(string s)
        {
            byte[] b = new byte[1024];
            b = Encoding.Unicode.GetBytes(s);
            SendAll(b);
        }
        private void SendAll(byte[] inbyte)
        {
            foreach (var item in SocClients.Values)
            {
                item.Send(inbyte);
            }
        }
        private void Send(IPEndPoint add, byte[] inbyte)
        {
            SocClients[add].Send(inbyte);
        }
        public void Send(IPEndPoint add, string s)
        {
            byte[] b = new byte[1024];
            b = Encoding.Unicode.GetBytes(s);
            Send(add, b);
        }
        public void Remove(IPEndPoint name)
        {
            SocClients[name].Shutdown(SocketShutdown.Both);
            SocClients[name].Close();
            SocClients[name].Dispose();
            SocClients.Remove(name);
        }
        public void Stop()
        {
            foreach (var item in SocClients.Values)
            {
                item?.Shutdown(SocketShutdown.Both);
                item?.Close();
                item?.Dispose();
            }

            SocServer?.Shutdown(SocketShutdown.Both);
            SocServer?.Close();
            SocServer?.Dispose();
            SocServer = null;
        }
    }
}

