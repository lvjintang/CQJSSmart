using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace CQJSSmart
{
    public delegate void NetDataReceivedEventHandler(INetPortDevice device, NetPortSerialDataEventArgs args);
    public class NetPortSerialDataEventArgs : EventArgs
    {
        // 摘要:
        //     The serial data received on the com port.
        public string SerialData = null;
        //
        // 摘要:
        //     String encoding format received from the device.
        public eStringEncoding SerialEncoding = eStringEncoding.eEncodingASCII;
    }
    public interface INetPortDevice
    {    
        //
        // 摘要:
        //     Net data receive event handler
        event NetDataReceivedEventHandler NetDataReceived;

        // 摘要:
        //     Function to send a string out of the ComPort.
        //
        // 参数:
        //   dataToTransmit:
        //     Serial data to send out
        //
        // 异常:
        //   System.ArgumentNullException:
        //     The specified string to transmit is 'null'.
        void Send(string dataToTransmit);
  
    }
}