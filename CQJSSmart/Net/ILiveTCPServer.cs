using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.CrestronThread;
using Crestron.SimplSharp.CrestronSockets;

namespace CQJSSmart
{
    public class TCPConnectionClient
    {
        public TCPServer ConnectionSocket;
        public uint clientindex;

       // public event NewConnectionEventHandler NewConnection;
        public event DataReceivedEventHandler DataReceived;
        public event DisconnectedEventHandler Disconnected;

        public void Read(TCPServer myTCPServer, uint clientIndex, int numberOfBytesReceived)
        {
            if (!myTCPServer.ClientConnected(clientindex)) return;

            string messageReceived = string.Empty;
            byte[] readBuffer = new byte[numberOfBytesReceived];
            Array.Copy(myTCPServer.GetIncomingDataBufferForSpecificClient(clientindex), readBuffer, numberOfBytesReceived);

            try
            {
                messageReceived = Encoding.GetEncoding(28591).GetString(readBuffer, 0, readBuffer.Length);


                if (DataReceived != null)
                {
                    DataReceived(this, messageReceived, EventArgs.Empty);
                }
                Array.Clear(readBuffer, 0, readBuffer.Length);
                myTCPServer.ReceiveDataAsync(clientindex, this.Read, 0);

            }
            catch (Exception)
            {

                if (Disconnected != null)
                    Disconnected(this, EventArgs.Empty);
            }
        }

       // myTCPServer.ReceiveDataAsync(clientindex, this.Read, 0);
    }
    #region 状态 委托
    public delegate void LogEventHandler(string Msg);
    /// <summary>
    /// 服务器状态 未连接，等待连接，连接已建立
    /// </summary>
    public enum ServerStatusLevel { Off, WaitingConnection, ConnectionEstablished };
    /// <summary>
    /// 连接建立委托
    /// </summary>
    /// <param name="e"></param>
    public delegate void NewConnectionEventHandler(EventArgs e);
    /// <summary>
    /// 数据接收委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    /// <param name="e"></param>
    public delegate void DataReceivedEventHandler(Object sender, string message, EventArgs e);
    /// <summary>
    /// 连接断开委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DisconnectedEventHandler(Object sender, EventArgs e);
    // public delegate void BroadcastEventHandler(string message, EventArgs e);

    #endregion

    public class ILiveTCPServer
    {
        /// <summary>
        /// 客户端列表
        /// </summary>
        List<TCPConnectionClient> connectionClienttList = new List<TCPConnectionClient>();


       public delegate void TcpDataHandler(string data);

        public event TcpDataHandler TcpDataEvent=null;

        TCPServer tcp = null;
        private uint clientindex;
        public ILiveTCPServer(int port)
        {
            tcp = new TCPServer("0.0.0.0",port,4096);
            tcp.SocketStatusChange += new TCPServerSocketStatusChangeEventHandler(tcp_SocketStatusChange);
   
        }

        void tcp_SocketStatusChange(TCPServer myTCPServer, uint clientIndex, SocketStatus serverSocketStatus)
        {
            if (serverSocketStatus==SocketStatus.SOCKET_STATUS_LINK_LOST||serverSocketStatus==SocketStatus.SOCKET_STATUS_NO_CONNECT)
            {
                tcp.DisconnectAll();
                this.Listen();
            }
           // throw new NotImplementedException();
        }


        public void Listen()
        {
            
           // while (true)
            {
                try
                {
                   // SocketErrorCodes codes = tcp.WaitForConnection("0.0.0.0", this.OnClientConnect);
                    SocketErrorCodes codes = tcp.WaitForConnectionAsync(this.OnClientConnect);
                   // ILiveDebug.Instance.WriteLine("tcplisten:" + tcp.PortNumber);
                }
                catch (Exception)
                {
                    // logger.Log(ex.Message);
                }


            }  
        }

        public void OnClientConnect(TCPServer myTCPServer, uint clientIndex)
        {
            if (myTCPServer.ClientConnected(clientIndex))
            {
                TCPConnectionClient socketConn = new TCPConnectionClient();
                socketConn.clientindex = clientIndex;
                socketConn.ConnectionSocket = myTCPServer;
                socketConn.DataReceived += new DataReceivedEventHandler(socketConn_DataReceived);
                socketConn.Disconnected += new DisconnectedEventHandler(socketConn_Disconnected);


                myTCPServer.ReceiveDataAsync(clientIndex, socketConn.Read, 0);


                connectionClienttList.Add(socketConn);
                //ClientConnected clientIndex
            }

        }

        void socketConn_Disconnected(object sender, EventArgs e)
        {
            ILiveDebug.Instance.WriteLine("socketConn_Disconnected");
            //throw new NotImplementedException();
        }

        void socketConn_DataReceived(object sender, string message, EventArgs e)
        {
            if (this.TcpDataEvent!=null)
            {
                this.TcpDataEvent(message);
            }
            
            //ILiveDebug.Instance.WriteLine("socketConn_DataReceived" + message);
        }


        internal void Send(string cmd)
        {
            byte[] cmdBytes = Encoding.GetEncoding(28591).GetBytes(cmd);

            foreach (TCPConnectionClient item in this.connectionClienttList)
            {
                if (!item.ConnectionSocket.ClientConnected(item.clientindex)) return;
                try
                {
                    item.ConnectionSocket.SendData(item.clientindex, cmdBytes, cmdBytes.Length);
                }
                catch (Exception)
                {

                    //  logger.Log(ex.Message);
                }
            }
        }

        
    }
}