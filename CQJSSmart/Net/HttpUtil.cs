using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace CQJSSmart
{
    public class HttpUtil
    {
        public string GetWeatherHtml()
        {
            string str = "http://php.weather.sina.com.cn/xml.php?city=%CE%C2%D6%DD&password=DJOYnieT8234jlsK&day=0";
            Crestron.SimplSharp.Net.Http.HttpClient client = new Crestron.SimplSharp.Net.Http.HttpClient();
            string html= client.Get(str, Encoding.UTF8);
            return html;
        }
        public string GetWenDu()
        {
            string str= this.GetWeatherHtml();
            str = str.Substring(0, str.IndexOf("</temperature1>"));
            int length=str.Length - str.IndexOf("<temperature1>")-14;
          //  ILiveDebug.Instance.WriteLine(str.IndexOf("<temperature1>").ToString() + " " + str.Length.ToString()+" "+length);

            string sss = str.Substring(str.IndexOf("<temperature1>")+14,length );
            return sss;
        }
    }
}