using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace maneu.tools.UptimeRobotClient
{
    public class UptimeRobotContext 
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

        /// <summary>
        /// Gets the details on any monitor ID that is passed in
        /// </summary>
        /// <param name="MonitorIds">List of IDs for the monitors you want to see</param>
        /// <returns>List of monitors matching passed in IDs</returns>
        public List<Monitor> GetMonitors( List<int> MonitorIds )
        {
            string id_delim = "-";
            string merged_ids = string.Join( id_delim, MonitorIds.ToArray() );

            return RequestMonitor( merged_ids );
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

            if (string.IsNullOrWhiteSpace(monitor.FriendlyName)
                || string.IsNullOrWhiteSpace(monitor.Url))
            {
                throw new ApplicationException("Some values are required for monitor creation.");
            }


            var sb = new StringBuilder(_baseUri);
            sb.Append("/newMonitor?");
            sb.AppendFormat("apiKey={0}", _apiKey);
            sb.AppendFormat("&monitorFriendlyName={0}", monitor.FriendlyName);
            //sb.AppendFormat("&monitorURL={0}", monitor.Url);
            sb.AppendFormat("&monitorType={0}", (int) monitor.Type);

            if (monitor.Type == MonitorType.Port)
            {
                sb.AppendFormat("&monitorSubType={0}", (int) monitor.Subtype);
                sb.AppendFormat("&monitorPort={0}", monitor.Port);
            }

            if (monitor.Type == MonitorType.Keyword)
            {
                sb.AppendFormat("&monitorKeywordType=1");
                sb.AppendFormat("&monitorKeywordValue={0}", monitor.KeywordValue);
            }


            sb.AppendFormat("&format={0}", _responseFormat);

            string result = string.Empty;

            try
            {
                var wc = new WebClient();
                result = wc.DownloadString(sb.ToString());
            }
            catch (WebException webex)
            {
                var ex = new UptimeRobotClientException(webex);
                ex.ExceptionType = UptimeRobotExceptionType.ServerError;
                throw ex;
            }
      
            

            XDocument xDoc = XDocument.Parse(result);

            // Error handling
            if(xDoc.Root.Name == "error")
            {
                var exception = new UptimeRobotClientException(null); 
                UptimeRobotExceptionType exType;
                
                string errorCode = (string) xDoc.Descendants().Select(doc => doc.Attribute("id")).FirstOrDefault();
                

                Enum.TryParse(errorCode, out exType);
                exception.ExceptionType = exType == 0 ? UptimeRobotExceptionType.ServerError : exType;
                throw exception;
            }

            monitor.Id = (int) xDoc.Descendants().Select(doc => doc.Attribute("id")).FirstOrDefault();
            monitor.CurrentStatus =
                (Status)
                Enum.Parse(typeof (Status),
                           (string) xDoc.Descendants().Select(doc => doc.Attribute("status")).FirstOrDefault());

            return monitor;
        }


        public void UpdateMonitor(Monitor monitor)
        {
//apiKey - required
//monitorID - required
//monitorFriendlyName -optional
//monitorURL -optional
//monitorType -optional
//monitorSubType -optional (used only for port monitoring)
//monitorPort -optional (used onlyfor port monitoring)
//monitorKeywordType - optional (used only for keyword monitoring)
//monitorKeywordValue - optional (used only for keyword monitoring)
//monitorHTTPUsername - optional
//monitorHTTPPasswırd - optional

            if (monitor.Id == 0)
            {
                throw new ApplicationException("Some values are required for monitor creation.");
            }


            var sb = new StringBuilder(_baseUri);
            sb.Append("/editMonitor?");
            sb.AppendFormat("apiKey={0}", _apiKey);
            sb.AppendFormat("&monitorID={0}", monitor.Id);
            if (!string.IsNullOrEmpty(monitor.FriendlyName))
                sb.AppendFormat("&monitorFriendlyName={0}", monitor.FriendlyName);
            if (!string.IsNullOrEmpty(monitor.Url))
                sb.AppendFormat("&monitorURL={0}", monitor.Url);

            sb.AppendFormat("&monitorType={0}", (int) monitor.Type);

            if (monitor.Type == MonitorType.Port)
            {
                sb.AppendFormat("&monitorSubType={0}", (int) monitor.Subtype);
                sb.AppendFormat("&monitorPort={0}", monitor.Port);
            }

            if (monitor.Type == MonitorType.Keyword)
            {
                sb.AppendFormat("&monitorKeywordType=1");
                sb.AppendFormat("&monitorKeywordValue={0}", monitor.KeywordValue);
            }


            sb.AppendFormat("&format={0}", _responseFormat);

            var wc = new WebClient();
            string result = wc.DownloadString(sb.ToString());

            XDocument xDoc = XDocument.Parse(result);

            // Error handling
            if (xDoc.Root.Name == "error")
            {
                var exception = new UptimeRobotClientException(null);
                UptimeRobotExceptionType exType;

                string errorCode = (string)xDoc.Descendants().Select(doc => doc.Attribute("id")).FirstOrDefault();


                Enum.TryParse(errorCode, out exType);
                exception.ExceptionType = exType == 0 ? UptimeRobotExceptionType.ServerError : exType;
                throw exception;
            }
        }

        public void DeleteMonitor(string monitorId)
        {
            var sb = new StringBuilder(_baseUri);
            sb.Append("/deleteMonitor?");
            sb.AppendFormat("apiKey={0}", _apiKey);
            sb.AppendFormat("&monitorID={0}", monitorId);

            var wc = new WebClient();
            string result = wc.DownloadString(sb.ToString());

            XDocument xDoc = XDocument.Parse(result);
            // Error handling
            if (xDoc.Root.Name == "error")
            {
                var exception = new UptimeRobotClientException(null);
                UptimeRobotExceptionType exType;

                string errorCode = (string)xDoc.Descendants().Select(doc => doc.Attribute("id")).FirstOrDefault();


                Enum.TryParse(errorCode, out exType);
                exception.ExceptionType = exType == 0 ? UptimeRobotExceptionType.ServerError : exType;
                throw exception;
            }
        }


        private List<Monitor> RequestMonitor(string monitorId = null)
        {
            var sb = new StringBuilder(_baseUri);
            sb.Append("/getMonitors?");
            sb.AppendFormat("apiKey={0}", _apiKey);
            if (monitorId != null)
            {
                sb.AppendFormat("&monitors={0}", monitorId);
            }

            sb.AppendFormat("&logs={0}", 1);
            sb.AppendFormat("&format={0}", _responseFormat);

            var wc = new WebClient();
            string result = wc.DownloadString(sb.ToString());

            XDocument xDoc = XDocument.Parse(result);


            // Error handling
            if (xDoc.Root.Name == "error")
            {
                var exception = new UptimeRobotClientException(null);
                UptimeRobotExceptionType exType;

                string errorCode = (string)xDoc.Descendants().Select(doc => doc.Attribute("id")).FirstOrDefault();


                Enum.TryParse(errorCode, out exType);
                exception.ExceptionType = exType == 0 ? UptimeRobotExceptionType.ServerError : exType;
                throw exception;
            }

            IEnumerable<Monitor> monitors = from m in xDoc.Descendants("monitor")
                                            select new Monitor
                                                       {
                                                           Id = Convert.ToInt32(m.Attribute("id").Value),
                                                           FriendlyName = m.Attribute("friendlyname").Value,
                                                           Type =
                                                               (MonitorType)
                                                               Enum.Parse(typeof (MonitorType),
                                                                          m.Attribute("type").Value),
                                                           Subtype =
                                                               m.Attribute("subtype").Value != ""
                                                                   ? (MonitorSubtype)
                                                                     Enum.Parse(typeof (MonitorSubtype),
                                                                                m.Attribute("subtype").Value)
                                                                   : MonitorSubtype.Unknow,
                                                           CurrentStatus =
                                                               (Status)
                                                               Enum.Parse(typeof (Status), m.Attribute("status").Value),
                                                           UptimeRatio =
                                                               Double.Parse(m.Attribute("alltimeuptimeratio").Value,
                                                                            NumberStyles.AllowDecimalPoint,
                                                                            CultureInfo.InvariantCulture),
                                                           KeywordValue = m.Attribute("keywordvalue").Value,
                                                           Port =
                                                               m.Attribute("port").Value == ""
                                                                   ? 0
                                                                   : Convert.ToInt32(m.Attribute("port").Value)
                                                       };

            return monitors.ToList();
        }
    }

    public enum ResponseFormat
    {
        Json,
        Xml
    }
}