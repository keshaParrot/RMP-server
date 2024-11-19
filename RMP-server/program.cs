using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.DependencyInjection;
using RMP_server.server;
using RMP_server.utils;
using System.Threading;
using System.Windows.Forms;
using System;

namespace RMP_server
{
    public class program
    {

        [STAThread] 
        static void Main()
        {
            TrayIconManager trayManager = new TrayIconManager();

            Thread workerThread = new Thread(() =>
            {
                try
                {       
                    HostApplicationBuilder builder = Host.CreateApplicationBuilder();
                    builder.Services.AddWindowsService(options =>
                    {
                        options.ServiceName = "RMPServer";
                    });

                    string sendOption = ConfigManager.GetDataReceptionMode();

                    builder.Services.AddSingleton<DataPacker>();
                    if (sendOption == "UART")
                    {
                        builder.Services.AddHostedService<SerialPortService>();
                        EventLogger.Log("set worker as SerialPortService.");
                    }
                    else if (sendOption == "TCPIP")
                    {
                        builder.Services.AddHostedService<TCPService>();
                        EventLogger.Log("set worker as DataServerService.");
                    }
                    else
                    {
                        EventLogger.Log("Error while creating service, was not providet option on config.");
                        Environment.Exit(-1);
                    }
                    IHost host = builder.Build();

                    host.Run();
                }
                catch (Exception ex)
                {
                    EventLogger.Log($"Error: {ex.Message}");
                }
            });

            workerThread.IsBackground = true;
            workerThread.Start();

            Application.Run();

            trayManager.Dispose();
        }
    }
}

