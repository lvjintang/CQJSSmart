using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.CrestronThread;
using WebSocketServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CQJSSmart
{
    public class IPadWebSocketServer
    {
       

        #region 初始化
        /// <summary>
        /// 数据接收委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public delegate void DataEventHandler(string service, JObject data);

        public event DataEventHandler DataReceived;
        private WebSocketServer.WebSocketServer WSServer = new WebSocketServer.WebSocketServer();
        /// <summary>
        /// 接收事件
        /// </summary>
        private Thread webSocketThread;

        public IPadWebSocketServer()
        {
        }
        public void Register()
        {
            webSocketThread = new Thread(WebSocketServerStart, null, Thread.eThreadStartOptions.Running);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        internal object WebSocketServerStart(object o)
        {

            WSServer.NewConnection += new WebSocketServer.NewConnectionEventHandler(WSServer_NewConnection);
            WSServer.Disconnected += new WebSocketServer.DisconnectedEventHandler(WSServer_Disconnected);
            WSServer.DataReceived += new WebSocketServer.DataReceivedEventHandler(Ipad_DataReceived);//this.DataReceived;
            WSServer.Log += new WebSocketServer.LogEventHandler(WSServer_Log);
            WSServer.StartServer();
            return o;
        }

        void WSServer_Log(string Msg)
        {
              ILiveDebug.Instance.WriteLine("WSLog" + DateTime.Now.ToShortTimeString() + ":" + Msg);
        }
        #endregion

        /// <summary>
        /// 客户端连接
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="e"></param>
        void WSServer_NewConnection(EventArgs e)
        {
            //ILiveDebug.Instance.WriteLine("IpadConnection" + DateTime.Now.ToShortTimeString() + ":" + e.ToString());
        }
        /// <summary>
        /// 客户端断开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WSServer_Disconnected(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void Ipad_DataReceived(object sender, string message, EventArgs e)
        {
            //{service: "FL_getTicketStatus", version: "1.0.0",data:{ticket: "APPQrcode_2c1yvQGplGkJ9isUVwZsie4pC1y7eu"}}
            try
            {
                JObject j = JObject.Parse(message);
                string version = j.Value<string>("version");
                string service = j.Value<string>("service");

                if (version != "1.0")
                {
                    this.WSServer_Send(this.BuildAnswerJson(service, new { data = "version error" }));
                    return;
                }

                if (this.DataReceived!=null)
                {
                    this.DataReceived(service, j.Value<JObject>("data"));
                }
               // switch (j.Value<string>("service"))
               // {
                //    case "scence":
                //        this.ScencePorcess(j.Value<string>("data"));
                //        break;
                //    case "light":
                //        this.LightProcess(j.Value<JObject>("data").Value<string>("zone"), j.Value<JObject>("data").Value<bool>("value"));
                //        this.ipad.WSServer_Send(this.BuildAnswerJson(j.Value<string>("service"), new { status = 1, zone = j.Value<JObject>("data").Value<string>("zone"), value = j.Value<JObject>("data").Value<bool>("value"), msg = "success" }));

                //        break;
                //    case "device":
                //        this.DevicePorcess(j.Value<JObject>("data").Value<string>("zone"), j.Value<JObject>("data").Value<bool>("value"));
                //        break;
                //    case "music":
                //        this.MusicProcess("play", j.Value<JObject>("data").Value<string>("zone"), j.Value<JObject>("data").Value<bool>("value"));
                //        break;
                //    default:
                //        break;
                //}
            }
            catch (Exception ex)
            {
                ILiveDebug.Instance.WriteLine("IPadWebSocketServerIpad_DataReceived:"+ex.Message);

            }
        }

        /// <summary>
        /// 向客户端发送执行结果
        /// </summary>
        /// <param name="service">功能SystemBusy</param>
        /// <param name="result">结果{IsBusy=true}</param>
        public void WSServer_Send(string service,object result)
        {
            WSServer.Send(this.BuildAnswerJson(service, result));
        }
        /// <summary>
        /// 向客户端发送字符串
        /// </summary>
        /// <param name="msg"></param>
        public void WSServer_Send(string msg)
        {
            WSServer.Send(msg);
        }
        #region 辅助
        private string BuildAnswerJson(string service, object result)
        {
            return this.BuildAnswerJson(new ResultService() { service = service, version = "1.0", result = result });
        }
        private string BuildAnswerJson(ResultService data)
        {
            return this.BuildAnswerJson(1, "ok", data);
        }
        private string BuildAnswerJson(int status, string msg, ResultService data)
        {
            WebResult j = new WebResult();
            j.status = status;
            j.msg = msg;
            j.result = data;
            return JsonConvert.SerializeObject(j);
        }
        public class WebResult
        {
            //{"status":1,"msg":"ok","result":{"service":"FL_getTicketStatus","version":"1.0.0","result":{"status":"0"}}}

            public int status = 0;
            public string msg { get; set; }
            public ResultService result { get; set; }
        }
        public class ResultService
        {
            public string service { get; set; }
            public string version { get; set; }
            public object result { get; set; }
        }
        #endregion

    }
}
