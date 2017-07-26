using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.CrestronThread;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CQJSSmart
{
    public class UISmart
    {
        //public delegate void DigitalPressHandler(bool button);
        //public event DigitalPressHandler DigitalPressEvent;

        private CrestronControlSystem controlSystem = null;
        private ILiveLogic _logic = null;


        public ILiveIpad ipad;

        #region 初始化
        public UISmart(CrestronControlSystem system, ILiveLogic logic)
        {
            this.controlSystem = system;
            this._logic = logic;
        }
        internal void Start()
        {
            this.RegisterDevices();
        }

        public void RegisterDevices()
        {
            #region 注册网络设备
            if (this.controlSystem.SupportsEthernet)
            {

                #region 注册IPad（WebSocket）
                this.ipad = new ILiveIpad(this._logic);
                //  this.ipad.DataReceived += new IPadWebSocketServer.DataEventHandler(Ipad_DataReceived);
                this.ipad.RegisterDevices();
                #endregion

            }
            #endregion

        }
        #endregion

        #region IPAD事件
        /// <summary>
        /// IPAD事件
        /// </summary>
        /// <param name="currentDevice"></param>
        /// <param name="args"></param>
        void mobile_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            ILiveDebug.Instance.WriteLine("mobile_SigChange" + args.Sig.Type + args.Sig.Number);
            switch (args.Sig.Type)
            {
                case eSigType.Bool:
                    {
                        this.ProcessMobileBool(args.Sig.Number, args.Sig.BoolValue);
                    }
                    break;
                case eSigType.UShort:
                    {
                        //this.ProcessTsw1052UShort(args.Sig.Number, args.Sig.UShortValue);
  
                    }
                    break;
            }

        }

        /// <summary>
        /// 触摸屏数字量处理
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        private void ProcessMobileBool(uint sigNumber, bool boolValue)
        {
            if (boolValue)
            {
                //CrestronMobileBool t = (CrestronMobileBool)sigNumber;
                //ILiveDebug.Instance.WriteLine("ProcessMobileBool" + t);
                //switch (t)
                //{
                //    #region 场景切换
                //    case CrestronMobileBool.Scence_Leave:
                //        //离开模式
                //        if (!this._logic.ScenceIsBusy)
                //        {
                //            new Thread(new ThreadCallbackFunction(this.ScenceLeave), this, Thread.eThreadStartOptions.Running);
                //        }
                //        break;
                //    case CrestronMobileBool.Scence_Show:
                //        //展示模式
                //        if (!this._logic.ScenceIsBusy)
                //        {
                //            new Thread(new ThreadCallbackFunction(this.ScenceShow), this, Thread.eThreadStartOptions.Running);

                //        }
                //        break;
                //    case CrestronMobileBool.Scence_XiuXi:
                //        //休息模式
                //        if (!this._logic.ScenceIsBusy)
                //        {
                //            new Thread(new ThreadCallbackFunction(this.ScenceXiuXi), this, Thread.eThreadStartOptions.Running);

                //        }
                //        break;
                //    #endregion
                //    #region 灯光
                //    case CrestronMobileBool.Light_AllOpen:
                //        if (!this._logic.light.LightScenceIsBusy)
                //        { 
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone1].BoolValue = true;
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone2].BoolValue = true;
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone3].BoolValue = true;
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone4].BoolValue = true;
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone5].BoolValue = true;
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone6].BoolValue = true;
                //            this._logic.light.LightAllOpen();
                //        }

                //        break;
                //    case CrestronMobileBool.Light_AllClose:
                //        if (!this._logic.light.LightScenceIsBusy)
                //        {              
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone1].BoolValue = false;
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone2].BoolValue = false;
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone3].BoolValue = false;
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone4].BoolValue = false;
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone5].BoolValue = false;
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone6].BoolValue = false;
                //            this._logic.light.LightAllClose();
                //        }

                //        break;
                //    case CrestronMobileBool.Light_AllShow:
                //        if (!this._logic.light.LightScenceIsBusy)
                //        {
                //            this._logic.light.LightAllShow();
                //        }
                //        break;
                //    case CrestronMobileBool.Light_XiuXi:
                //        if (!this._logic.light.LightScenceIsBusy)
                //        {
                //            this._logic.light.LightXiuXi();
                //        }
                //        break;
                //    case CrestronMobileBool.Light_Zone1:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone1].BoolValue)
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone1].BoolValue = true;
                //            this._logic.light.LightZone1On();
                //        }
                //        else
                //        { 
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone1].BoolValue = false;
                //            this._logic.light.LightZone1Off();
                //        }
                //        break;
                //    case CrestronMobileBool.Light_Zone2:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone2].BoolValue)
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone2].BoolValue = true;
                //            this._logic.light.LightZone2On();
                //        }
                //        else
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone2].BoolValue = false;
                //            this._logic.light.LightZone2Off();            
                //        }
                //        break;
                //    case CrestronMobileBool.Light_Zone3:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone3].BoolValue)
                //        { 
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone3].BoolValue = true;
                //            this._logic.light.LightZone3On();
                //        }
                //        else
                //        {
                //           this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone3].BoolValue = false;
                //            this._logic.light.LightZone3Off();
                //        }
                //        break;
                //    case CrestronMobileBool.Light_Zone4:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone4].BoolValue)
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone4].BoolValue = true;
                //            this._logic.light.LightZone4On();
                            
                //        }
                //        else
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone4].BoolValue = false;                           
                //            this._logic.light.LightZone4Off();

                //        }
                //        break;
                //    case CrestronMobileBool.Light_Zone5:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone5].BoolValue)
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone5].BoolValue = true;
                //            this._logic.light.LightZone5On();
                            
                //        }
                //        else
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone5].BoolValue = false;
                //            this._logic.light.LightZone5Off();
                            
                //        }
                //        break;
                //    case CrestronMobileBool.Light_Zone6:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone6].BoolValue)
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone6].BoolValue = true;
                //            this._logic.light.LightZone6On();
                            
                //        }
                //        else
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Light_Zone6].BoolValue = false;
                //            this._logic.light.LightZone6Off();
                //        }
                //        break;
                //    #endregion
                //    #region 多媒体
                //    case CrestronMobileBool.Media_On:
                //        if (!this._logic.MediaIsBusy)
                //        {
                //            new Thread(new ThreadCallbackFunction(this.MediaOn), this, Thread.eThreadStartOptions.Running);
                //        }

                //        break;
                //    case CrestronMobileBool.Media_Off:
                //        if (!this._logic.MediaIsBusy)
                //        {
                //            new Thread(new ThreadCallbackFunction(this.MediaOff), this, Thread.eThreadStartOptions.Running);

                //        }
                //        break;
                //    #region 电子沙盘
                //    case CrestronMobileBool.ShaPan:
                //        if (!this._logic.ShaPanIsBusy)
                //        {
                //            if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.ShaPan].BoolValue)
                //            {
                //                this.mobile.BooleanInput[(uint)CrestronMobileBool.ShaPan].BoolValue = true;

                //                this._logic.ShaPanOn();
                //            }
                //            else
                //            {
                //                this.mobile.BooleanInput[(uint)CrestronMobileBool.ShaPan].BoolValue = false;

                //                this._logic.ShaPanOff();
                //            }
                //        }
   
                //        break;
                //    case CrestronMobileBool.ShaPanVolUp:
                //        if (!this._logic.ShaPanIsBusy)
                //        {
                //            this._logic.ShaPanVolUp(); 
                //        }
                        
                //        break;
                //    case CrestronMobileBool.ShaPanVolDown:
                //        if (!this._logic.ShaPanIsBusy)
                //        {
                //            this._logic.ShaPanVolDown();
                //        }
                //        break;
                //    #endregion
                //    #region 广告机
                //    case CrestronMobileBool.GuangGao42:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao42].BoolValue)
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao42].BoolValue = true;
                //            this._logic.AD42Open();

                //        }
                //        else
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao42].BoolValue = false;
                //            this._logic.AD42Close();
        
                //        }
                //        break;
                //    case CrestronMobileBool.GuangGao32:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao32].BoolValue)
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao32].BoolValue = true;
                //            this._logic.AD32Open();
                          
                //        }
                //        else
                //        { 
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao32].BoolValue = false;
                //            this._logic.AD32Close();
                           
                //        }
                //        break;
                //    case CrestronMobileBool.GuangGao22:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao22].BoolValue)
                //        { 
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao22].BoolValue = true;
                //            this._logic.AD22Open();
                           
                //        }
                //        else
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao22].BoolValue = false;
                //            this._logic.AD22Close();
                            
                //        }
                //        break;
                //    case CrestronMobileBool.GuangGao80:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao80].BoolValue)
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao80].BoolValue = true;
                //             this._logic.AD80Open();
                            
                //        }
                //        else
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.GuangGao80].BoolValue = false;
                //              this._logic.AD80Close();
                           
                //        }
                //        break;
                //    #endregion
                //    #region LED
                //    case CrestronMobileBool.LED:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.LED].BoolValue)
                //        { 
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.LED].BoolValue = true;
                //            this._logic.LEDOn();
                           
                //        }
                //        else
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.LED].BoolValue = false;  
                //            this._logic.LEDOff();
                          
                //        }
                //        break;

                //    #endregion
                //    #region 电视墙
                //    case CrestronMobileBool.TV:
                //        if (!this._logic.TVIsBusy)
                //        {
                //            if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.TV].BoolValue)
                //            {
                //                this.mobile.BooleanInput[(uint)CrestronMobileBool.TV].BoolValue = true;

                //                this._logic.TVOn();
                //            }
                //            else
                //            {
                //                this.mobile.BooleanInput[(uint)CrestronMobileBool.TV].BoolValue = false;

                //                this._logic.TVOff();
                //            }
                //        }
                //        break;
                //    case CrestronMobileBool.TVVolUp:
                //        if (!this._logic.TVIsBusy)
                //        {
                //            this._logic.TVVolUp();
                //        }
                //        break;
                //    case CrestronMobileBool.TVVolDown:
                //        if (!this._logic.TVIsBusy)
                //        {
                //            this._logic.TVVolDown();
                //        }
                //        break;
                //    #endregion
                //    #endregion

                //    #region 语音讲解
                //    case CrestronMobileBool.MusicPower:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            new Thread(new ThreadCallbackFunction(this.MusicPower), this, Thread.eThreadStartOptions.Running);
                //        }
                //        break;
                //    case CrestronMobileBool.MusicPlay1:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            this._logic.MusicPlay1();
                //        }
                //        break;
                //    case CrestronMobileBool.MusicPlay2:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            this._logic.MusicPlay2();
                //        }
                //        break;
                //    case CrestronMobileBool.MusicPlay3:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            this._logic.MusicPlay3();
                //        }
                //        break;
                //    case CrestronMobileBool.MusicPlay4:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            this._logic.MusicPlay4();
                //        }
                //        break;
                //    case CrestronMobileBool.MusicPlay5:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            this._logic.MusicPlay5();
                //        }
                //        break;
                //    case CrestronMobileBool.MusicPlay6:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            this._logic.MusicPlay6();
                //        }
                //        break;
                //    case CrestronMobileBool.MusicPlay7:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            this._logic.MusicPlay7();
                //        }
                //        break;
                //    case CrestronMobileBool.MusicPlay12:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            this._logic.MusicPlay12();
                //        }
                //        break;
                //    case CrestronMobileBool.MusicPlay:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            this._logic.MusicPlay();
                //        }
                //        break;
                //    case CrestronMobileBool.MusicPause:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            this._logic.MusicPause();
                //        }
                //        break;
                //    case CrestronMobileBool.MusicVolUp:
                //        if (!this._logic.MusicIsBusy)
                //        {
                //            this._logic.MusicVolUp();
                //        } break;
                //    case CrestronMobileBool.MusicVolDown:
                //        this._logic.MusicVolDown();
                //        break;
                //    //YuYin_Zone1Play=60,
                //    //YuYIn_Zone1Stop=61,
                //    //YuYin_Zone2Play = 62,
                //    //YuYIn_Zone2Stop = 63,
                //    //YuYin_Zone3Play = 64,
                //    //YuYIn_Zone3Stop = 65,
                //    //YuYin_Zone4Play = 66,
                //    //YuYIn_Zone4Stop = 67,
                //    //YuYin_Zone5Play = 68,
                //    //YuYIn_Zone5Stop = 69,
                //    //YuYin_Zone6Play = 70,
                //    //YuYIn_Zone6Stop = 71,
                //    #endregion
                    
                //    #region 空调
                //    case CrestronMobileBool.Temp:
                //        if (!this.mobile.BooleanInput[(uint)CrestronMobileBool.Temp].BoolValue)
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Temp].BoolValue = true;
                //            this._logic.temp.TempOn();

                //        }
                //        else
                //        {
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Temp].BoolValue = false;

                //            this._logic.temp.TempOff();
                //        }
                //        break;
                //    case CrestronMobileBool.Temp_On:

                //            this._logic.temp.TempOn();
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Temp].BoolValue = true;
                //            break;
                //    case CrestronMobileBool.Temp_Off:
   
                //            this._logic.temp.TempOff();
                //            this.mobile.BooleanInput[(uint)CrestronMobileBool.Temp].BoolValue = false;
                //        break;
                //    #endregion

                //    default:
                //        break;
                //}
            }

        }

        //void Ipad_DataReceived(string service, string data)
        //{
        //    //{service: "FL_getTicketStatus", version: "1.0.0",data:{ticket: "APPQrcode_2c1yvQGplGkJ9isUVwZsie4pC1y7eu"}}

        //    try
        //    {

        //        switch (service)
        //        {
        //            case "scence":
        //                this.ScencePorcess(data.ToString());
        //                break;
        //            case "light":


        //                this.LightProcess(JObject.Parse(data).Value<string>("zone"), JObject.Parse(data).Value<bool>("value"));
        //                break;
        //            case "device":
        //                this.DevicePorcess(JObject.Parse(data).Value<string>("zone"), JObject.Parse(data).Value<bool>("value"));
        //                break;
        //            case "music":
        //                this.MusicProcess("play", JObject.Parse(data).Value<string>("zone"), JObject.Parse(data).Value<bool>("value"));
        //                break;
        //            default:
        //                break;
        //        }

        //      /*  JObject j = JObject.Parse(message);
        //        if (j.Value<string>("version") != "1.0")
        //        {
        //            this.ipad.WSServer_Send(this.BuildAnswerJson(j.Value<string>("service"), new { status = 0, msg = "version error" }));
        //            return;
        //        }

        //        switch (j.Value<string>("service"))
        //        {
        //            case "scence":
        //                this.ScencePorcess(j.Value<string>("data"));
        //                break;
        //            case "light":
        //                this.LightProcess(j.Value<JObject>("data").Value<string>("zone"), j.Value<JObject>("data").Value<bool>("value"));
        //                this.ipad.WSServer_Send(this.BuildAnswerJson(j.Value<string>("service"), new { status = 1, zone = j.Value<JObject>("data").Value<string>("zone"),value=j.Value<JObject>("data").Value<bool>("value"), msg = "success" }));

        //                break;
        //            case "device":
        //                this.DevicePorcess(j.Value<JObject>("data").Value<string>("zone"), j.Value<JObject>("data").Value<bool>("value"));
        //                break;
        //            case "music":
        //                this.MusicProcess("play",j.Value<JObject>("data").Value<string>("zone"), j.Value<JObject>("data").Value<bool>("value"));
        //                break;
        //            default:
        //                break;
        //        }*/
        //    }
        //    catch (Exception ex)
        //    {
        //        ILiveDebug.Instance.WriteLine(ex.Message);
        //    }
        //}
        ///// <summary>
        ///// 场景处理
        ///// </summary>
        ///// <param name="name"></param>
        //private void ScencePorcess(string name)
        //{
        //    switch (name)
        //    {
        //        case "show":
        //            if (!this._logic.ScenceIsBusy)
        //            {
        //                new Thread(new ThreadCallbackFunction(this.ScenceShow), this, Thread.eThreadStartOptions.Running);

        //            }
        //            else
        //            {
        //                this.ipad.WSServer_Send("SystemBusy", new { status = 0, msg = "SystemBusy" });

        //            }
        //            break;
        //        case "xiuxi":
        //            //休息模式
        //            if (!this._logic.ScenceIsBusy)
        //            {
        //                new Thread(new ThreadCallbackFunction(this.ScenceXiuXi), this, Thread.eThreadStartOptions.Running);

        //            }
        //            else
        //            {
        //                this.ipad.WSServer_Send("SystemBusy", new { status = 0, msg = "SystemBusy" });

        //            }
        //            break;
        //        case "leave":
        //            //离开模式
        //            if (!this._logic.ScenceIsBusy)
        //            {
        //                new Thread(new ThreadCallbackFunction(this.ScenceLeave), this, Thread.eThreadStartOptions.Running);
        //            }
        //            break;
        //        case "LightOn":
        //            if (!this._logic.light.LightScenceIsBusy)
        //            {

        //                this.ipad.WSServer_Send("Scence", "LightOn");
        //                this._logic.light.LightAllOpen();
        //                this.c2nicb_InRoom.Feedbacks[1].State = true;
        //                this.c2nicb_InRoom.Feedbacks[2].State = true;
        //                this.c2nicb_OutRoom.Feedbacks[1].State = true;
        //                this.c2nicb_OutRoom.Feedbacks[2].State = true;
        //            }
        //            else
        //            {
        //                this.ipad.WSServer_Send("SystemBusy", new { status = 0, msg = "SystemBusy" });

        //            }
        //            break;
        //        case "LightOff":
        //            if (!this._logic.light.LightScenceIsBusy)
        //            {

        //                this.ipad.WSServer_Send("Scence", "LightOff");
        //                this._logic.light.LightAllOpen();
        //                this.c2nicb_InRoom.Feedbacks[1].State = false;
        //                this.c2nicb_InRoom.Feedbacks[2].State = false;
        //                this.c2nicb_OutRoom.Feedbacks[1].State = false;
        //                this.c2nicb_OutRoom.Feedbacks[2].State = false;
        //            }
        //            else
        //            {
        //                this.ipad.WSServer_Send("SystemBusy", new { status = 0, msg = "SystemBusy" });

        //            }
        //            break;
        //        case "LightShow":
        //            if (!this._logic.light.LightScenceIsBusy)
        //            {
        //                this.ipad.WSServer_Send("Scence", "LightShow");
        //                this._logic.light.LightAllShow();
        //            }
        //            else
        //            {
        //                this.ipad.WSServer_Send("SystemBusy", new { status = 0, msg = "SystemBusy" });

        //            }
        //            break;
        //        case "LightXiuXi":
        //            if (!this._logic.light.LightScenceIsBusy)
        //            {
        //                this.ipad.WSServer_Send("Scence", "LightXiuXi");
        //                this._logic.light.LightXiuXi();
        //            }
        //            else
        //            {
        //                this.ipad.WSServer_Send("SystemBusy", new { status = 0, msg = "SystemBusy" });

        //            }
        //            break;
        //        case "MediaOn":
        //            if (!this._logic.MediaIsBusy)
        //            {
        //                new Thread(new ThreadCallbackFunction(this.MediaOn), this, Thread.eThreadStartOptions.Running);
        //            }
        //            else
        //            {
        //                this.ipad.WSServer_Send("SystemBusy", new { status = 0, msg = "SystemBusy" });

        //            }
                  
        //            break;
        //        case "MediaOff":

        //            if (!this._logic.MediaIsBusy)
        //            {
        //                new Thread(new ThreadCallbackFunction(this.MediaOff), this, Thread.eThreadStartOptions.Running);

        //            }
        //            else
        //            {
        //                this.ipad.WSServer_Send("SystemBusy", new { status = 0, msg = "SystemBusy" });

        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}
        ///// <summary>
        ///// 灯光控制
        ///// </summary>
        ///// <param name="zone"></param>
        ///// <param name="value"></param>
        //private void LightProcess(string zone,bool value)
        //{
        //    this.ipad.WSServer_Send("Light", new { zone = zone, value = value });

        //    if (value)
        //    {
        //        switch (zone)
        //        {
        //            case "zone1":
        //                this._logic.light.LightZone1On();
                        
        //                break;
        //            case "zone2":
        //                this._logic.light.LightZone2On();
        //                break;
        //            case "zone3":
        //                this._logic.light.LightZone3On();
        //                break;
        //            case "zone4":
        //                this._logic.light.LightZone4On();
        //                break;
        //            case "zone5":
        //                this._logic.light.LightZone5On();
        //                break;
        //            case "zone6":
        //                this._logic.light.LightZone6On();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        switch (zone)
        //        {
        //            case "zone1":
        //                this._logic.light.LightZone1Off();
        //                break;
        //            case "zone2":
        //                this._logic.light.LightZone2Off();
        //                break;
        //            case "zone3":
        //                this._logic.light.LightZone3Off();
        //                break;
        //            case "zone4":
        //                this._logic.light.LightZone4Off();
        //                break;
        //            case "zone5":
        //                this._logic.light.LightZone5Off();
        //                break;
        //            case "zone6":
        //                this._logic.light.LightZone6Off();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        ///// <summary>
        ///// 多媒体设备控制
        ///// </summary>
        ///// <param name="zone"></param>
        ///// <param name="value"></param>
        //private void DevicePorcess(string zone, bool value)
        //{
        //    if (value)
        //    {
        //        switch (zone)
        //        {
        //            case "zone1":
        //                this._logic.AD42Open();
        //                break;
        //            case "zone2":
        //                this._logic.TVOn();
        //                break;
        //            case "zone3":
        //                this._logic.LEDOn();
        //                break;
        //            case "zone4":
        //                this._logic.AD80Open();
        //                break;
        //            case "zone5":
        //                this._logic.AD32Open();
        //                break;
        //            case "zone6":
        //                this._logic.AD22Open();
        //                break;
        //            case "zone7":
        //                this._logic.ShaPanOn();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        switch (zone)
        //        {
        //            case "zone1":
        //                this._logic.AD42Close();
        //                break;
        //            case "zone2":
        //                this._logic.TVOff();
        //                break;
        //            case "zone3":
        //                this._logic.LEDOff();
        //                break;
        //            case "zone4":
        //                this._logic.AD80Close();
        //                break;
        //            case "zone5":
        //                this._logic.AD32Close();
        //                break;
        //            case "zone6":
        //                this._logic.AD22Close();
        //                break;
        //            case "zone7":
        //                this._logic.ShaPanOff();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        ///// <summary>
        ///// 语音控制
        ///// </summary>
        ///// <param name="fun"></param>
        ///// <param name="zone"></param>
        ///// <param name="value"></param>
        //private void MusicProcess(string fun,string zone, bool value)
        //{
        //    if (fun=="play")
        //    {
        //        #region 播放控制
        //        if (value)
        //        {
        //            switch (zone)
        //            {
        //                case "zone1":
        //                    this._logic.light.LightZone1On();
        //                    break;
        //                case "zone2":
        //                    this._logic.light.LightZone2On();
        //                    break;
        //                case "zone3":
        //                    this._logic.light.LightZone3On();
        //                    break;
        //                case "zone4":
        //                    this._logic.light.LightZone4On();
        //                    break;
        //                case "zone5":
        //                    this._logic.light.LightZone5On();
        //                    break;
        //                case "zone6":
        //                    this._logic.light.LightZone6On();
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //        else
        //        {
        //            switch (zone)
        //            {
        //                case "zone1":
        //                    this._logic.light.LightZone1Off();
        //                    break;
        //                case "zone2":
        //                    this._logic.light.LightZone2Off();
        //                    break;
        //                case "zone3":
        //                    this._logic.light.LightZone3Off();
        //                    break;
        //                case "zone4":
        //                    this._logic.light.LightZone4Off();
        //                    break;
        //                case "zone5":
        //                    this._logic.light.LightZone5Off();
        //                    break;
        //                case "zone6":
        //                    this._logic.light.LightZone6Off();
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //        #endregion
        //    }
  
        //}

        #region Json处理

        #endregion

        #endregion
    }


}