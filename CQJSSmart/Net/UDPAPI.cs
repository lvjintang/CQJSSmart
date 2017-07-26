using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;

namespace CQJSSmart
{
    public class UDPAPI
    {
        public static string h = "192.168.1.41";
        public static int p = 6890;

        public static void SendData(string data)
        {
            byte[] sendBytes = Encoding.ASCII.GetBytes(data);
            UDPClient client = new UDPClient(h,0, p);
            client.Connect();
            client.SendData(sendBytes);
            client.DisConnect();
        }

        public static void SendData(string host, int port, string data)
        {
      
            byte[] sendBytes = Encoding.ASCII.GetBytes(data);
            UDPClient client = new UDPClient(host,0, port);
            client.Connect();
            client.SendData(sendBytes);
            client.DisConnect();
        }
        public static void SendData(string host, int port, byte[] sendBytes)
        {
            UDPClient client = new UDPClient(host,0, port);
            client.Connect();
            client.SendData(sendBytes);
            client.DisConnect();
        }

    }
    
}