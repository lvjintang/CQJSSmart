using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace CQJSSmart
{
    /// <summary>
    /// 主要用于转换快思聪COM口
    /// </summary>
    public class ILiveComPort:INetPortDevice
    {
        private ComPort com = null;
        /// <summary>
        /// COM口
        /// </summary>
        /// <param name="com"></param>
        public ILiveComPort(ComPort com)
        {
            this.com = com;
            this.com.SerialDataReceived += new ComPortDataReceivedEvent(com_SerialDataReceived);
        }

        public void Register()
        {
            if (!this.com.Registered)
            {
                if (this.com.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                    ErrorLog.Error("COM Port couldn't be registered. Cause: {0}", this.com.DeviceRegistrationFailureReason);
                if (this.com.Registered)
                    this.com.SetComPortSpec(ComPort.eComBaudRates.ComspecBaudRate9600,
                                                                     ComPort.eComDataBits.ComspecDataBits8,
                                                                     ComPort.eComParityType.ComspecParityNone,
                                                                     ComPort.eComStopBits.ComspecStopBits1,
                                         ComPort.eComProtocolType.ComspecProtocolRS232,
                                         ComPort.eComHardwareHandshakeType.ComspecHardwareHandshakeNone,
                                         ComPort.eComSoftwareHandshakeType.ComspecSoftwareHandshakeNone,
                                         false);
            }
        }
        void com_SerialDataReceived(ComPort ReceivingComPort, ComPortSerialDataEventArgs args)
        {
            if (this.NetDataReceived!=null)
            {
                this.NetDataReceived(this, new NetPortSerialDataEventArgs() { SerialData = args.SerialData, SerialEncoding = args.SerialEncoding });
            }
        }
        #region INetPortDevice 成员

        public event NetDataReceivedEventHandler NetDataReceived;

        public void Send(string dataToTransmit)
        {
            if (this.com!=null)
            {
                try
                {
                    this.com.Send(dataToTransmit);
                }
                catch (Exception)
                {
                    
                }
            }
        }

        #endregion
    }
}