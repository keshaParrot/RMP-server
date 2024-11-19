using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using Newtonsoft.Json;
using RMP_server.collectors;
using RMP_server.data;
using RMP_server.utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace RMP_server.server
{
    public class TCPService : BackgroundService
    {
        private const int Port = 12345;
        private static string IpAddress = "192.168.0.64";

        private TcpListener listener;
        private readonly ILogger<TCPService> _logger;
        private readonly DataPacker _dataService;

        public TCPService(DataPacker dataService, ILogger<TCPService> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                IpAddress = GetHostAddress() ?? "127.0.0.1";
                listener = new TcpListener(IPAddress.Parse(IpAddress), Port);
                listener.Start();

                _logger.LogInformation("Server started, listening on {IpAddress}:{Port}", IpAddress, Port);
                EventLogger.Log($"Server started, listening on {IpAddress}:{Port}");

                while (!stoppingToken.IsCancellationRequested)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    _logger.LogInformation("Client connected.");
                    EventLogger.Log("Client connected.");

                    _ = HandleClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while starting the server.");
                EventLogger.Log($"Error: {ex.Message}");
            }
            finally
            {
                listener?.Stop();
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                NetworkStream networkStream = client.GetStream();
                SystemData systemData = _dataService.packAllData();

                string jsonData = JsonConvert.SerializeObject(systemData);
                StreamWriter writer = new StreamWriter(networkStream);
                {
                    await writer.WriteAsync(jsonData);
                    await writer.FlushAsync();
                }

                _logger.LogInformation("Data sent to the client.");
                EventLogger.Log("Data sent to the client.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while handling the client.");
                EventLogger.Log($"Client handling error: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        private static string GetHostAddress()
        {
            string userIP = ConfigManager.GetIpAddress();
            if (!string.IsNullOrEmpty(userIP))
            {
                return userIP;
            }
            EventLogger.Log("user ip is not providet");
            return null;
        }
    }
}
