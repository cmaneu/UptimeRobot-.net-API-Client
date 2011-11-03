using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace maneu.tools.UptimeRobotClient
{
    public class UptimeRobotContext : IDisposable
    {
        private readonly string _baseUri = "http://api.uptimerobot.com";

        private string _apiKey;
        private ResponseFormat _responseFormat = ResponseFormat.Xml;
        
        public UptimeRobotContext(string apiKey)
        {
            _apiKey = apiKey;
        }

        public string ApiKey
        {
            get { return _apiKey; }
            set { _apiKey = value; }
        }

        public List<Monitor> GetMonitors()
        {
            return RequestMonitor();
        }

        public Monitor GetMonitor(string monitorId)
        {
            return RequestMonitor(monitorId).FirstOrDefault();
        }

        public Monitor AddMonitor(Monitor monitor)
        {
            
//            •apiKey - required
//•monitorFriendlyName - required
//•monitorURL - required
//•monitorType - required
//•monitorSubType - optional (required for port monitoring)
//•monitorPort - optional (required for port monitoring)
//•monitorKeywordType - optional (required for keyword monitoring)
//•monitorKeywordValue - optional (required for keyword monitoring)
//•monitorHTTPUsername - optional
//•monitorHTTPPasswırd - optional

            if(    string.IsNullOrWhiteSpace(monitor.FriendlyName)
                || string.IsNullOrWhiteSpace(monitor.Url))
            {
                throw new ApplicationException("Some values are required for monitor creation.");
            }


            StringBuilder sb = new StringBuilder(_baseUri);
            sb.Append("/newMonitor?");
            sb.AppendFormat("apiKey={0}", _apiKey);
            sb.AppendFormat("&monitorFriendlyName={0}", monitor.FriendlyName);
            sb.AppendFormat("&monitorURL={0}", monitor.Url);
            sb.AppendFormat("&monitorType={0}", (int)monitor.Type);
            
            if (monitor.Type == MonitorType.Port)
            {
                sb.AppendFormat("&monitorSubType={0}",(int) monitor.Subtype);
                sb.AppendFormat("&monitorPort={0}", monitor.Port);
            }

            if (monitor.Type == MonitorType.Keyword)
            {
                sb.AppendFormat("&monitorKeywordType=1");
                sb.AppendFormat("&monitorKeywordValue={0}", monitor.KeywordValue);
            }

           
            sb.AppendFormat("&format={0}", _responseFormat);

            WebClient wc = new WebClient();
            string result = wc.DownloadString(sb.ToString());

            XDocument xDoc = XDocument.Parse(result);


            monitor.Id = (int) xDoc.Descendants().Select(doc => doc.Attribute("id")).FirstOrDefault();
            monitor.CurrentStatus = (Status) Enum.Parse(typeof(Status),(string) xDoc.Descendants().Select(doc => doc.Attribute("status")).FirstOrDefault());
            
            return monitor;
        }

        public void UpdateMonitor()
        {
            
        }

        public void DeleteMonitor()
        {}


        private List<Monitor> RequestMonitor(string monitorId = null)
        {
            StringBuilder sb = new StringBuilder(_baseUri);
            sb.Append("/getMonitors?");
            sb.AppendFormat("apiKey={0}", _apiKey);
            if (monitorId != null)
            {
                sb.AppendFormat("&monitors={0}", monitorId);    
            }

            sb.AppendFormat("&logs={0}", 1);
            sb.AppendFormat("&format={0}", _responseFormat);

            WebClient wc = new WebClient();
            string result = wc.DownloadString(sb.ToString());

            XDocument xDoc = XDocument.Parse(result);

            var monitors = from m in xDoc.Descendants("monitor")
                           select new Monitor()
                           {
                               Id = Convert.ToInt32(m.Attribute("id").Value),
                               FriendlyName = m.Attribute("friendlyname").Value,
                               Type = (MonitorType)Enum.Parse(typeof(MonitorType), m.Attribute("type").Value),
                               Subtype = m.Attribute("subtype").Value != "" ? (MonitorSubtype)Enum.Parse(typeof(MonitorSubtype), m.Attribute("subtype").Value) : MonitorSubtype.Unknow,
                               CurrentStatus = (Status)Enum.Parse(typeof(Status), m.Attribute("status").Value),
                               UptimeRatio = Double.Parse(m.Attribute("alltimeuptimeratio").Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture),
                               KeywordValue = m.Attribute("keywordvalue").Value,
                               Port = m.Attribute("port").Value == "" ? 0 : Convert.ToInt32(m.Attribute("port").Value)
                           };

            return monitors.ToList();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public enum ResponseFormat
    {Json, Xml}

}
