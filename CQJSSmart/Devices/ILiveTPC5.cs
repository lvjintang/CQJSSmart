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
        public delegate void PTCIHandler(int id, int btnid);

        public event PTCIHandler PushTPCIEvent;

        INetPortDevice iserver = null;
        public ILiveTPC5(INetPortDevice port)
        {
            try
            {
                this.iserver = port;
                this.iserver.NetDataReceived += new NetDataReceivedEventHandler(iserver_NetDataReceived);
            }
            catch (Exception ex)
            {
                ILiveDebug.Instance.WriteLine(ex.Message);
            }
        }

        void iserver_NetDataReceived(INetPortDevice device, NetPortSerialDataEventArgs args)
        {
            OnDataReceived(args.SerialData);
          //  throw new NotImplementedException();
        }

    

        private void Read(UDPServer myUDPServer, int numberOfBytesReceived)
        {
            byte[] rbytes = new byte[numberOfBytesReceived];

            if (numberOfBytesReceived > 0)
            {

                //Array.Copy(myUDPServer.IncomingDataBuffer, rbytes, numberOfBytesReceived);

                string messageReceived = Encoding.GetEncoding(28591).GetString(myUDPServer.IncomingDataBuffer, 0, numberOfBytesReceived);


                OnDataReceived(messageReceived);
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
             ILiveDebug.Instance.WriteLine("TPCDebug:"+ILiveUtil.ToHexString(sendBytes));

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
                if (rdata.Count == 6 && rdata[0] == 0x55 && rdata[5] == 0x0D)
                {

                    byte iChanIdx = rdata[1];

                    int h = rdata[2];

                    int l = rdata[3];

                    // if (rdata[4] == 0x0D)//校验
                    {
                        this.PushTPCIEvent(iChanIdx, (h * 256) + l);
                    }

                }
        
                rdata.Clear();

            }
            catch (Exception ex)
            {
                ILiveDebug.Instance.WriteLine(ex.Message);

            }


        }


    }
}