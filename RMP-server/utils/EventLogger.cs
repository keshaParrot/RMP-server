using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMP_server.server;

namespace RMP_server.utils
{
    public static class EventLogger
    {
        public static void Log(string message)
        {
            string logFilePath = "RMPServerServiceLogs.txt";
            try
            {
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("RMPServer", $"Log write error: {ex.Message}", EventLogEntryType.Error);

            }
            finally { 
                Console.WriteLine($"Log write {message}");
            }
        }
    }
}
