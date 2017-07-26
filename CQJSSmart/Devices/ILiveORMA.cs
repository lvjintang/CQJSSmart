using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace CQJSSmart
{
    /// <summary>
    /// ORMA5.1 解码器
    /// </summary>
    public class ILiveORMA
    {
        IROutputPort irORMA51 = null;
        public ILiveORMA(IROutputPort smart)
        {
            this.irORMA51 = smart;//书房空调

            string file = Crestron.SimplSharp.CrestronIO.Directory.GetApplicationDirectory() + "\\IR\\OMAR51.ir";
            try
            {
                uint i = irORMA51.LoadIRDriver(file);
              // ILiveDebug.WriteLine("dirver"+i);
            }
            catch (Exception ex)
            {
                
                 ILiveDebug.Instance.WriteLine(ex.Message);
            }
           
        }
        #region 5.1解码器
        public void InputSub()
        {
            this.irORMA51.Press("INPUTSUB");
        }
        public void InputAdd()
        {
            this.irORMA51.Press("INPUTADD");
        }

        public void MODE1()
        {
            this.irORMA51.Press("MODE1");
        }
        public void MODE2()
        {
            this.irORMA51.Press("MODE2");
        }
        public void MODE3()
        {
            this.irORMA51.Press("MODE3");
        }
        public void MODE4()
        {
            this.irORMA51.Press("MODE4");
        }
        public void MusicSub()
        {
            this.irORMA51.Press("MUSICSUB");
        }
        public void MusicAdd()
        {
            this.irORMA51.Press("MUSICADD");
        }
        public void MicSub()
        {
            this.irORMA51.Press("MICSUB");
        }
        public void MicAdd()
        {
            this.irORMA51.Press("MICADD");
        }
        public void EffectSub()
        {
            this.irORMA51.Press("EFFECTSUB");
        }
        public void EffectAdd()
        {
            this.irORMA51.Press("EFFECTADD");
        }
        #endregion

    }
}