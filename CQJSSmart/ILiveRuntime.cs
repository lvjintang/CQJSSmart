using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace CQJSSmart
{
    public class ILiveRuntime
    {
        private CrestronControlSystem _controlSystem = null;

        ILiveLogic logic = null;

        UISmart ui = null;
        public ILiveRuntime(CrestronControlSystem system)
        {
            this._controlSystem = system;

            this.Init();

        }
        /// <summary>
        /// 注册所有模块设备
        /// </summary>
        private void Init()
        {
            this.logic = new ILiveLogic(this._controlSystem);
            ui = new UISmart(this._controlSystem,logic);
        }

        internal void StartServices()
        {
            //启动调试服务
            ILiveDebug.Instance.StartDebug("192.168.188.138", 8801,8801);
            ILiveDebug.Instance.DebugDataReceived = client_DebugDataReceived;

        }
        #region 系统调试
        void client_DebugDataReceived(INetPortDevice device, NetPortSerialDataEventArgs args)
        {
            switch (args.SerialData)
            {
                case "t1":
                  //  this.logic.light.LivingAllOn();
                    break;
                case "t2":
                   // this.logic.light.LivingAllOff();

                    break;
                case "t3":
                    break;
                default:
                    break;
            }
        }
        #endregion

    }
}