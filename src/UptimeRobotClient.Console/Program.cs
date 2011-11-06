using System;
using System.Collections.Generic;
using System.Configuration;
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


            // Test GetMonitors()
            //System.Console.WriteLine("1. Get all monitors");

            UptimeRobotContext context = new UptimeRobotContext(ConfigurationManager.AppSettings["uptimerobot-id"]);

            //var monitors = context.GetMonitors();

            //foreach (var m in monitors)
            //{
            //    System.Console.WriteLine(" -[{0}] {2} {1}",m.CurrentStatus, m.FriendlyName, m.Id);
            //}

            //System.Console.WriteLine("--------------------" + Environment.NewLine);
           

            //// Test GetMonitor(monitorId)
            //System.Console.WriteLine("2. Get a monitor by id");

            //var monitor = context.GetMonitor(monitors.FirstOrDefault().Id.ToString());
            //System.Console.WriteLine(" -[{0}] {1}", monitor.CurrentStatus, monitor.FriendlyName);

            //System.Console.WriteLine("--------------------" + Environment.NewLine);
           

            // Test AddMonitor
            System.Console.WriteLine("3. Create a new monitor");

            Monitor newMonitor = new Monitor();
            newMonitor.FriendlyName = "Google home page";
            newMonitor.Type = MonitorType.Http;
            newMonitor.Url = "http://www.google.com";

            context.AddMonitor(newMonitor);

            System.Console.WriteLine("--------------------" + Environment.NewLine);

            // Test UpdateMonitor

            //System.Console.WriteLine("4. Modify an existing monitor");
            //System.Console.Write("Enter the Id of the monitor to modify : ");
            //string monitorIdToModify = System.Console.ReadLine();

            //var monitorToModify = context.GetMonitor(monitorIdToModify);
            //monitorToModify.FriendlyName += "test";
            //context.UpdateMonitor(monitorToModify);
            //monitorToModify = context.GetMonitor(monitorIdToModify);
            //System.Console.WriteLine("New monitor name : " + monitorToModify.FriendlyName);


            //// Test Delete Monitor
            //System.Console.WriteLine("--------------------" + Environment.NewLine);
            //System.Console.WriteLine("5. Delete a monitor monitor");
            //System.Console.Write("Enter the Id of the monitor to delete : ");
            //string monitorIdToDelete = System.Console.ReadLine();
            //context.DeleteMonitor(monitorIdToDelete);
            ////

            System.Console.Read();
        }
    }
}
