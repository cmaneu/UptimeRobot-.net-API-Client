using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace maneu.tools.UptimeRobotClient
{
    public class Monitor
    {
        /// <summary>
        /// The unique Id of the monitor
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The friendly name of the monitor
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// The monitor type (http, ping, ...)
        /// </summary>
        public MonitorType Type { get; set; }

        /// <summary>
        /// The detailed monitor type (http/https, ...)
        /// </summary>
        public MonitorSubtype Subtype { get; set; }

        /// <summary>
        /// The value of the keyword used for keyword monitoring.
        /// </summary>
        public string KeywordValue { get; set; }

        /// <summary>
        /// The credentials used for http monitoring
        /// </summary>
        public NetworkCredential HttpCredentials { get; set; }

        /// <summary>
        /// The port monitored, if monitor type is "Port monitoring"
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The current status of the monitor
        /// </summary>
        public Status CurrentStatus { get; set; }

        /// <summary>
        /// The uptime percentage of the monitor
        /// </summary>
        public double UptimeRatio { get; set; }
    }

    public enum MonitorType
    {
        Http = 1,
        Keyword = 2,
        Ping = 3,
        Port = 4
    }

    public enum MonitorSubtype
    {
        Unknow,
        Http = 1,
        Https = 2,
        FTP = 3,
        SMTP = 4,
        POP3 = 5,
        IMAP = 6,
        CustomPort = 99
    }

    public enum Status
    {
        Pause = 0,
        Up = 2,
        SeemsDown = 8,
        Down = 9
    }
}
