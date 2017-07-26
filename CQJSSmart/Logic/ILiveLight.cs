using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace CQJSSmart
{
    /// <summary>
    /// 大视听室
    /// </summary>
    public class ILiveLight
    {

        ILiveIsinRelay isin6 = null;
        UDPClient port = null;
        public ILiveLight(CrestronControlSystem system)
        {
            port = new UDPClient("192.168.188.21", 6004, 8004);
            port.NetDataReceived += new NetDataReceivedEventHandler(port_NetDataReceived);
            port.Connect();
            isin6 = new ILiveIsinRelay(3, port);
        }

        void port_NetDataReceived(INetPortDevice device, NetPortSerialDataEventArgs args)
        {
            ILiveDebug.Instance.WriteLine("lightrec:"+ILiveUtil.ToHexString(Encoding.GetEncoding(28591).GetBytes(args.SerialData)));
        }
        
        /// <summary>
        /// 客厅全开
        /// </summary>
        public void LivingAllOn()
        {
            ILiveDebug.Instance.WriteLine("lighton1");
         //   isinDimmer1.SetDim1(255);
            //客厅玄关筒灯 1:1
            //客厅灯带 6：10
            //客厅吊灯 4:3
            //客厅壁灯 4:2


        }
        /// <summary>
        /// 客厅全关
        /// </summary>
        public void LivingAllOff()
        {
       

        }
        
    }
}