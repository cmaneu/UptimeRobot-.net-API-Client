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
            System.Console.WriteLine("UptimeRobot .net API Client test application.");
            System.Console.WriteLine();

            System.Console.WriteLine("1. Get all monitors");

            UptimeRobotContext client = new UptimeRobotContext(YOUR_API_KEY_HERE);

            var monitors = client.GetMonitors();

            foreach (var monitor in monitors)
            {
                System.Console.WriteLine(" -[{0}] {1}",monitor.CurrentStatus, monitor.FriendlyName);
            }

            System.Console.Read();
        }
    }
}
