using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using maneu.tools.UptimeRobotClient;

namespace UptimeRobotClient.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            UptimeRobotContext client = new UptimeRobotContext("u14318-80f7f5bbb5bc5902473ea7f8");

            var monitors = client.GetMonitors();

            foreach (var monitor in monitors)
            {
                System.Console.WriteLine(" -[{0}] {1}",monitor.CurrentStatus, monitor.FriendlyName);
            }

            System.Console.Read();
        }
    }
}
