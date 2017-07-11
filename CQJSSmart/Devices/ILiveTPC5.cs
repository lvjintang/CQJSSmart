using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;
using Crestron.SimplSharp.CrestronSockets;

namespace CQJSSmart
{

    /// <summary>
    /// 施德朗5寸触摸屏
    /// </summary>
    public class ILiveTPC5
    {
        TCPClient tcpClient = null;
       

        private ComPort com;

        public delegate void PTCIHandler(int id, int btnid);

        public event PTCIHandler PushTPCIEvent;

        public ILiveTPC5(int port)
        {
            tcpClient = new TCPClient("192.168.188.25", 8004, 4096);
            tcpClient.SocketStatusChange+=new TCPClientSocketStatusChangeEventHandler(tcpClient_SocketStatusChange);
            this.Connect();

        }
        private void Connect()
        {
            try
            {
                if (tcpClient.ClientStatus!=SocketStatus.SOCKET_STATUS_CONNECTED)
                {
                    SocketErrorCodes codes = tcpClient.ConnectToServer();
                }
                
            }
            catch (Exception ex)
            {
                //ILiveDebug.Instance.WriteLine("tpcdebug:" + ex.Message);
            }

            new Thread(tcpListenMethod, null, Thread.eThreadStartOptions.Running);
        }

        void tcpClient_SocketStatusChange(TCPClient myTCPClient, SocketStatus clientSocketStatus)
        {
   
            if (clientSocketStatus!=SocketStatus.SOCKET_STATUS_CONNECTED)
            {
                try
                {
                    myTCPClient.ConnectToServer();
                    new Thread(tcpListenMethod, null, Thread.eThreadStartOptions.Running);
                }
                catch (Exception)
                {
                    
                }
           
            }
           
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 数据队列处理
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal object tcpListenMethod(object obj)
        {
            tcpClient.ReceiveDataAsync(this.Read);
           
            return null;
        }

        public void Read(TCPClient myTCPClient, int numberOfBytesReceived)
        {
            if (myTCPClient.ClientStatus != SocketStatus.SOCKET_STATUS_CONNECTED)
            {

                return;
            }
            string messageReceived = string.Empty;
          
            try
            {

                messageReceived = Encoding.GetEncoding(28591).GetString(myTCPClient.IncomingDataBuffer, 0, numberOfBytesReceived);
               
                OnDataReceived(messageReceived);
     
                myTCPClient.ReceiveDataAsync(this.Read, 0);


            }
            catch (Exception ex)
            {

               // if (Disconnected != null)
                 //   Disconnected(this, EventArgs.Empty);
            }
        }

        public ILiveTPC5(ComPort com)
        {
            #region 注册串口
            this.com = com;
            com.SerialDataReceived += new ComPortDataReceivedEvent(ILiveTPC5_SerialDataReceived);
            if (!com.Registered)
            {
                if (com.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                    ErrorLog.Error("COM Port couldn't be registered. Cause: {0}", com.DeviceRegistrationFailureReason);
                if (com.Registered)
                    com.SetComPortSpec(ComPort.eComBaudRates.ComspecBaudRate9600,
                                                                     ComPort.eComDataBits.ComspecDataBits8,
                                                                     ComPort.eComParityType.ComspecParityNone,
                                                                     ComPort.eComStopBits.ComspecStopBits1,
                                         ComPort.eComProtocolType.ComspecProtocolRS232,
                                         ComPort.eComHardwareHandshakeType.ComspecHardwareHandshakeNone,
                                         ComPort.eComSoftwareHandshakeType.ComspecSoftwareHandshakeNone,
                                         false);
            }

            #endregion
        }
        List<byte> rdata = new List<byte>(6);

        void ILiveTPC5_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {
            this.OnDataReceived(args.SerialData);
            //if (rdata.Count>50)
            //{
            //    rdata.Clear();
            //}
            //byte[] sendBytes = Encoding.ASCII.GetBytes(args.SerialData);
            //ILiveDebug.Instance.WriteLine("485Length:" + sendBytes.Length.ToString() + "data:" + ILiveUtil.ToHexString(sendBytes));

            //try
            //{
            //    foreach (var item in sendBytes)
            //    {
            //        rdata.Add(item);
            //        if (item == 0x0D&&rdata.Count>5)
            //        {
            //            this.ProcessData();
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ILiveDebug.Instance.WriteLine(ex.Message);

            //   // throw;
            //}


        }

        void OnDataReceived(string serialData)
        {
            if (rdata.Count > 50)
            {
                rdata.Clear();
            }
            byte[] sendBytes = Encoding.ASCII.GetBytes(serialData);
           // ILiveDebug.Instance.WriteLine("485Length:" + sendBytes.Length.ToString() + "data:" + ILiveUtil.ToHexString(sendBytes));
           // ILiveDebug.Instance.WriteLine("TPCDebug:"+ILiveUtil.ToHexString(sendBytes));

            try
            {
                foreach (var item in sendBytes)
                {
                    rdata.Add(item);
                    if (item == 0x0D && rdata.Count > 5)
                    {
                        this.ProcessData();
                    }
                }

            }
            catch (Exception ex)
            {
                ILiveDebug.Instance.WriteLine(ex.Message);

                // throw;
            }
        }
        void ProcessData()
        {
            try
            {
                if (rdata.Count == 6 && rdata[0]==0x55&&rdata[1]==0x10)
                {

                    byte iChanIdx = rdata[2];

                    int h = rdata[3];

                    int l = rdata[4];

                    if (rdata[5] == 0x0D)
                    {
                        this.PushTPCIEvent(iChanIdx, (h*256) + l);
                    }

                }
                //ILiveDebug.Instance.WriteLine("queuecount" + rdata.Count);

                rdata.Clear();

            }
            catch (Exception ex)
            {
                ILiveDebug.Instance.WriteLine(ex.Message);

            }
 
          
        }


    }
}