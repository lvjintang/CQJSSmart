using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace CQJSSmart
{
    public class ILiveLogic
    {
        private CrestronControlSystem controlSystem = null;
       // public ILiveLight light = new ILiveLight();

        //public ILiveVivitek vivitek=new ILiveVivitek()
        public ILiveLogic(CrestronControlSystem system)
        {
            this.controlSystem = system;
        }
        /// <summary>
        /// 影院模式
        /// </summary>
        public void ScenceMovie()
        { }
        /// <summary>
        /// 离开模式
        /// </summary>
        public void ScenceLeave()
        { }
        /// <summary>
        /// 打扫模式
        /// </summary>
        public void ScenceClear()
        { }
    }
}