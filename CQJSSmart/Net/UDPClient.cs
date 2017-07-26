using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;

namespace CQJSSmart
{
    public class UDPClient:INetPortDevice
    {
        public UDPServer server = null;

        public string host = string.Empty;
        public int remoteport = 0;
        public int localport = 0;
        public UDPClient(string host,int localport, int remoteport)
        {
            // Socket clientSocket = new Socket(Crestron.SimplSharp.AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
            this.host = host;
            this.localport = localport;
            this.remoteport = remoteport;
            server = new UDPServer();
        }
        public void Connect()
        {
            try
            {
                server.EnableUDPServer(this.host,this.localport, this.remoteport);

                SocketErrorCodes code = server.ReceiveDataAsync(this.Read);
            }
            catch (Exception)
            {

 
            }
        }
        private void Read(UDPServer myUDPServer, int numberOfBytesReceived)
        {
            byte[] rbytes = new byte[numberOfBytesReceived];

            if (numberOfBytesReceived > 0)
            {

        
                string messageReceived = Encoding.GetEncoding(28591).GetString(myUDPServer.IncomingDataBuffer, 0, numberOfBytesReceived);

                NetDataReceived(this, new NetPortSerialDataEventArgs() { SerialData = messageReceived, SerialEncoding = eStringEncoding.eEncodingASCII });
               // OnDataReceived(messageReceived);
                // CrestronConsole.PrintLine("messageReceived:" + messageReceived);

            }
            try
            {
                // CrestronConsole.PrintLine("Recv:" + ILiveUtil.ToHexString(rbytes));
                SocketErrorCodes code = myUDPServer.ReceiveDataAsync(this.Read);
                Thread.Sleep(300);
            }
            catch (Exception)
            {


            }
        }

  
        public void DisConnect()
        {
            server.DisableUDPServer();
        }
        public void SendData(byte[] sendbytes)
        {


            server.SendData(sendbytes, sendbytes.Length);

        }
        public byte[] RecevedData()
        {
            byte[] rbytes = { };
            if (server.ReceiveData() > 0)
            {
                rbytes = server.IncomingDataBuffer;
            }

            return rbytes;
        }

        #region INetPortDevice 成员

        public event NetDataReceivedEventHandler NetDataReceived;

        public void Send(string data)
        {

            byte[] sendBytes = Encoding.GetEncoding(28591).GetBytes(data);
            this.SendData(sendBytes);
           // throw new NotImplementedException();
        }

        #endregion
    }
}