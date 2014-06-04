using System;

namespace maneu.tools.UptimeRobotClient
{
    public class AlertContact
    {
        public string Id { get; set; }

        public AlertContactType Type { get; set; }

        public string Value { get; set; }

        public AlertContactStatus Status { get; set; }
    }

    public enum AlertContactType
    {
        Sms = 1,
        Email = 2,
        Twitter = 3,
        Boxcar = 4,
        WebHook = 5,
        Pushbullet = 6
    }

    public enum AlertContactStatus
    {
        NotActivated = 0,
        Paused = 1,
        Active = 2
    }
}