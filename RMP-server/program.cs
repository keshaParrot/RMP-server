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

                    builder.Services.AddSingleton<DataService>();
                    builder.Services.AddHostedService<DataServerService>();

                    IHost host = builder.Build();

                    host.Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            });

            workerThread.IsBackground = true;
            workerThread.Start();

            Application.Run();

            trayManager.Dispose();
        }
    }
}

