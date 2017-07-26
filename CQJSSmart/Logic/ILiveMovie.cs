using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace CQJSSmart
{
    public class ILiveMovie
    {
        private CrestronControlSystem controlSystem = null;
        ILiveIsinRelay isin6 = null;
        ILiveTPC5 tpc5 = null;
        /// <summary>
        /// 电源时序器
        /// </summary>
        private Relay relayPower = null;
        /// <summary>
        /// 高尔夫主机
        /// </summary>
        private Relay relayGolf = null;
        /// <summary>
        /// 网络机顶盒（红外）
        /// </summary>
        private IROutputPort irTV = null;
        /// <summary>
        /// 功放（3合一）
        /// </summary>
        private IROutputPort irAV = null;
        /// <summary>
        /// 蓝光机
        /// </summary>
        private IROutputPort irBlueray = null;

        private ILiveComPort comLight = null;
        private ILiveComPort comProjector = null;
      //  private ComPort comLight = null;

        UDPClient port = null;
        public ILiveMovie(CrestronControlSystem system)
        {
            this.controlSystem = system;

            this.comLight = new ILiveComPort(this.controlSystem.ComPorts[1]);
            this.comLight.Register();

            this.comProjector = new ILiveComPort(this.controlSystem.ComPorts[2]);
            this.comProjector.Register();

            this.relayPower=this.controlSystem.RelayPorts[1];
            this.relayGolf = this.controlSystem.RelayPorts[2];
            this.irTV = this.controlSystem.IROutputPorts[1];
            this.irAV = this.controlSystem.IROutputPorts[2];
            this.irBlueray = this.controlSystem.IROutputPorts[3];

            this.irTV.LoadIRDriver("");

            isin6 = new ILiveIsinRelay(comLight);
            this.tpc5 = new ILiveTPC5(comLight);
        //    port = new UDPClient("192.168.188.22", 6004, 8004);
        //    port.NetDataReceived += new NetDataReceivedEventHandler(port_NetDataReceived);
         //   port.Connect();
         //   bigvivitek = new ILiveVivitek(port);
        }

        void comTouch_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {
            //throw new NotImplementedException();
        }
        void comProjector_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 看电影
        /// </summary>
        public void MoviesOn()
        {
            this.relayPower.Open();//开电源
            //开投影
            this.comProjector.Send("PWR ON\r");
            this.irBlueray.Press("PowerOn");//开碟机
            this.irAV.Press("HDMI1");//切换信号至HDMI1

        }
        /// <summary>
        /// 唱歌
        /// </summary>
        public void KTVOn()
        {
            this.relayPower.Open();
        }
        /// <summary>
        /// 高尔夫
        /// </summary>
        public void GolfOn()
        {
            this.relayPower.Close();
        }
        /// <summary>
        /// 看电视
        /// </summary>
        public void TVOn()
        {
            this.relayPower.Open();
        }
        /// <summary>
        /// 全关
        /// </summary>
        public void AllOff()
        {
            this.comProjector.Send("PWR OFF\r");
            this.relayPower.Close();
        }
        void port_NetDataReceived(INetPortDevice device, NetPortSerialDataEventArgs args)
        {
         //   ILiveDebug.Instance.WriteLine("movie:" + ILiveUtil.ToHexString(Encoding.GetEncoding(28591).GetBytes(args.SerialData)));
        }
    }
}