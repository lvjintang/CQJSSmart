using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace CQJSSmart
{
    public class ILiveDebug
    {
        public static readonly ILiveDebug Instance = new ILiveDebug();

        public NetDataReceivedEventHandler DebugDataReceived
        {
            set
            {
                client.NetDataReceived += value;
            }
        }

        private UDPClient client = null;
        private bool EnableDebug = false;

        private ILiveDebug()
        {
        }
        public void StartDebug(string ip,int localport,int remoteport)
        {
            if (this.EnableDebug==false)
            {
                client = new UDPClient(ip, localport, remoteport);
                client.Connect();
                
                this.EnableDebug = true;
                this.WriteLine("Debug Open");
            }
    
        }




        public void StopDebug()
        {
            if (this.EnableDebug)
            {
                client.DisConnect();
                client = null;
                this.EnableDebug = false;
                this.WriteLine("Debug Close");
            }

        }

        #region 发送数据
        private void SendData(string data)
        {
            //CrestronConsole.PrintLine(data);

            if (this.EnableDebug)
            {
                client.Send(data);
            }


        }
        public void WriteLine(string msg)
        {
            this.SendData(msg + "\r\n");
        }
        public void WriteLine(string msg, params object[] args)
        {
            string message = string.Format(msg, args);
            this.SendData(message);
        }
        #endregion

    }
}