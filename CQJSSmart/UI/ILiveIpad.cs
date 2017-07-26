using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;
using Crestron.SimplSharpPro.CrestronThread;

namespace CQJSSmart
{
    public class ILiveIpad
    {
        private ILiveLogic _logic = null;

        public IPadWebSocketServer ipad;
        public ILiveIpad(ILiveLogic logic)
        {
            this._logic = logic;
        }
        
        public void RegisterDevices()
        {

            #region 注册网络设备

                #region 注册IPad（WebSocket）
                this.ipad = new IPadWebSocketServer();
                this.ipad.DataReceived += new IPadWebSocketServer.DataEventHandler(Ipad_DataReceived);
                this.ipad.Register();

                #endregion
            #endregion

        }

        void music8250_MusicScenceEvent(bool val)
        {
            this.ipad.WSServer_Send("MusicPower", new { state = val });
        }

        void media_MediaZoneEvent(int zone, bool vl)
        {
            this.ipad.WSServer_Send("Media", new { zone = zone, state = vl });
        }

        void _logic_ScenceEvent(string evt)
        {
            this.ipad.WSServer_Send("Scence", new { data = evt });
        }

        void light_ZoneLightEvent(int zone, bool vl)
        {
            this.ipad.WSServer_Send("Light", new { zone = zone, state = vl });
        }


        #region IPAD事件
        void Ipad_DataReceived(string service, JObject data)
        {
            //{service: "FL_getTicketStatus", version: "1.0.0",data:{ticket: "APPQrcode_2c1yvQGplGkJ9isUVwZsie4pC1y7eu"}}

            try
            {

                switch (service)
                {
                    case "scence":
                        this.ScencePorcess(data.Value<string>("data"));
                        break;
                    case "light":
                       // ILiveDebug.Instance.WriteLine(data);
                        this.LightProcess(data.Value<string>("zone"), data.Value<bool>("value"));
                        break;
                    case "media":
                        this.DevicePorcess(data.Value<string>("zone"), data.Value<bool>("value"));
                        break;
                    case "musicplay":
                        this.MusicProcess("play", data.Value<string>("data"));
                        break;
                    case "music":
                        this.MusicProcess("control", data.Value<string>("data"));
                        break;
                    case "MusicPower":
                        this.MusicProcess(data.Value<bool>("value"));
                        break;
                    case "sp":
                        this.SPProcess(data.Value<string>("data"));
                        break;
                    case "tv":
                        this.TVProcess(data.Value<string>("data"));
                        break;
                    case "Ipad":
                        this.SendInitData(data.Value<string>("data"));
                        break;
                    case "temp":
                        this.TempProcess(data.Value<string>("data"));
                        break;
                  
                    default:
                        break;
                }

             
            }
            catch (Exception ex)
            {
                ILiveDebug.Instance.WriteLine(ex.Message);
            }
        }

        private void TempProcess(string p)
        {
            switch (p)
            {
                case "on":
                    //this._logic.temp.TempOn();

                    break;
                case "off":
                    //this._logic.temp.TempOff();
                    break;
                   

                default:
                    break;
            }
        }

        private void SendInitData(string p)
        {
            if (p=="Init")
            {
                //this._logic.light.Zone1State = this._logic.light.Zone1State;
                //this._logic.light.Zone2State = this._logic.light.Zone2State;
                //this._logic.light.Zone3State = this._logic.light.Zone3State;
                //this._logic.light.Zone4State = this._logic.light.Zone4State;
                //this._logic.light.Zone5State = this._logic.light.Zone5State;
                //this._logic.light.Zone6State = this._logic.light.Zone6State;

                //this._logic.media.Zone1State = this._logic.media.Zone1State;
                //this._logic.media.Zone2State = this._logic.media.Zone2State;
                //this._logic.media.Zone3State = this._logic.media.Zone3State;
                //this._logic.media.Zone4State = this._logic.media.Zone4State;
                //this._logic.media.Zone5State = this._logic.media.Zone5State;
                //this._logic.media.Zone6State = this._logic.media.Zone6State;
                //this._logic.media.Zone7State = this._logic.media.Zone7State;
                //this._logic.media.music8250.MusicIsOn = this._logic.media.music8250.MusicIsOn;
            }
        }
        /// <summary>
        /// 场景处理
        /// </summary>
        /// <param name="name"></param>
        private void ScencePorcess(string name)
        {
            switch (name)
            {
                case "MediaOff":

                    //if (!this._logic.MediaIsBusy)
                    //{
                    //    new Thread(new ThreadCallbackFunction(this._logic.MediaOff), this, Thread.eThreadStartOptions.Running);

                    //}
                    //else
                    //{
                    //    this.ipad.WSServer_Send("SystemBusy", new { status = 0, msg = "SystemBusy" });

                    //}
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 灯光控制
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="value"></param>
        private void LightProcess(string zone, bool value)
        {
            //this.ipad.WSServer_Send("Light", new { zone = zone, value = value });

            if (value)
            {
                switch (zone)
                {
                    case "zone1":
                       // this._logic.light.LightZone1On();

                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (zone)
                {
                    case "zone1":
                        //this._logic.light.LightZone1Off();
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 多媒体设备控制
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="value"></param>
        private void DevicePorcess(string zone, bool value)
        {
            //this.ipad.WSServer_Send("Light", new { zone = zone, value = value });
            if (value)
            {
                switch (zone)
                {
                    case "zone1":
                        //this._logic.media.AD42Open();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (zone)
                {
                    case "zone1":
                        //this._logic.media.AD42Close();
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 语音控制
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="zone"></param>
        /// <param name="value"></param>
        private void MusicProcess(string fun, string zone)
        {
       

        }
        private void SPProcess(string fun)
        {
            if (fun == "volup")
            {
               // this._logic.media.ShaPanVolUp();
            }
            else if (fun == "voldown")
            {
                //this._logic.media.ShaPanVolDown();
            }
        }
        private void TVProcess(string fun)
        {
            if (fun == "volup")
            {
                //this._logic.media.TVVolUp();
            }
            else if (fun == "voldown")
            {
               // this._logic.media.TVVolDown();
            }
        }
        /// <summary>
        /// 语音控制
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="zone"></param>
        /// <param name="value"></param>
        private void MusicProcess(bool fun)
        {

            if (fun)
            {
                //this._logic.media.music8250.ZoneOn();
            }
            else
            {
                //this._logic.media.music8250.ZoneOff();
            }
        }
        #region Json处理

        #endregion

        #endregion
    }
}