using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace maneu.tools.UptimeRobotClient
{
    public class UptimeRobotClientException : Exception
    {
        public UptimeRobotClientException(Exception innerEx)
            :base("Unable to execute the request.",innerEx)
        {
      
        }

        public UptimeRobotExceptionType ExceptionType { get; set; }
    }


//    100	apiKey not mentioned or in a wrong format
//101	apiKey is wrong
//102	format is wrong (should be xml or json)
//103	No such method exists
//200	monitorID(s) should be integers
//201	monitorUrl is invalid
//202	monitorType is invalid
//203	monitorSubType is invalid
//204	monitorKeywordType is invalid
//205	monitorPort is invalid
//206	monitorFriendlyName is required
//207	The monitor already exists
//208	monitorSubType is required for this type of monitors
//209	monitorKeyWordType and monitorKeyWordValue are required for this type of monitors
//210	monitorID doesn't exist
//211	monitorID is required
//212	The account has no monitors
//213	At least one of the parameters to be edited are required
    public enum UptimeRobotExceptionType
    {
        ServerError = 1,
        ApiKeyNotWellFormed = 100,
        ApiKeyWrong = 101, 
        MonitorIdInvalid = 200, 
        MonitorUrlInvalid = 201, 
        MonitorPortInvalid = 205,
        MonitorExisting = 207,
        MonitorSubtypeRequired = 208,
        MonitorKeywordRequired = 209,
        MonitorIdUnknow = 210, 
        MonitorIdRequired = 211,
        NoMonitorsInAccount = 212
    }
}
