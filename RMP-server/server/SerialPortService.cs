using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RMP_server.data;
using RMP_server.utils;

namespace RMP_server.server
{
    public class SerialPortService : BackgroundService
    {
        private readonly ILogger<SerialPortService> _logger;
        private readonly DataPacker _dataService;

        public SerialPortService(DataPacker dataService, ILogger<SerialPortService> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (SerialPort serialPort = new SerialPort(getCOMPort(), 9600))
            {
                try
                {
                    serialPort.Open();
                    _logger.LogInformation("Serial port opened. Waiting for requests.");
                    EventLogger.Log("Serial port opened. Waiting for requests.");

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        if (serialPort.BytesToRead > 0)
                        {
                            string request = serialPort.ReadLine();
                            _logger.LogInformation($"Received request: {request}");
                            EventLogger.Log($"Received request: {request}");

                            if (request == "GET_DATA")
                            {
                                SystemData systemData = _dataService.packAllData();
                                string json = JsonConvert.SerializeObject(systemData);

                                serialPort.WriteLine(json);
                                _logger.LogInformation($"Data sent: {json}");
                                EventLogger.Log($"Data sent: {json}");
                            }
                            else
                            {
                                _logger.LogWarning($"Unknown request: {request}");
                                EventLogger.Log($"Unknown request: {request}");
                            }
                        }

                        if (!stoppingToken.IsCancellationRequested)
                        {
                            SystemData systemData = _dataService.packAllData();
                            string json = JsonConvert.SerializeObject(systemData);

                            serialPort.WriteLine(json);
                            _logger.LogInformation($"Data sent: {json}");
                            EventLogger.Log($"Data sent: {json}");

                            await Task.Delay(getSengInterval(), stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error with serial port: {ex.Message}");
                    EventLogger.Log($"Error with serial port: {ex.Message}");
                }
                finally
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Close();
                        _logger.LogInformation("Serial port closed.");
                        EventLogger.Log("Serial port closed.");
                    }
                }
            }
        }
        private static string getCOMPort()
        {
            string COMPort = ConfigManager.GetComPort();
            if (!string.IsNullOrEmpty(COMPort))
            {
                return COMPort;
            }
            EventLogger.Log("COM port is not providet");
            Environment.Exit(-1);
            return null;
        }
        private static int getSengInterval()
        {
            return ConfigManager.GetnIterval();
        }
    }
}
