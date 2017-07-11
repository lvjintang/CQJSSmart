using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace CQJSSmart
{
    public class ILiveSmartAPI
    {
        private CrestronControlSystem _controlSystem = null;

        public ILiveSmartAPI(CrestronControlSystem system)
        {
            this._controlSystem = system;

            this.Init();

        }
        /// <summary>
        /// 注册所有模块设备
        /// </summary>
        private void Init()
        {
        }

        internal void StartServices()
        {
            //throw new NotImplementedException();
        }
    }
}