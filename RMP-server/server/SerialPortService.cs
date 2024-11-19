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
            SerialPort serialPort = new SerialPort("COM3", 9600);
            serialPort.Open();

            if (serialPort.IsOpen)
            {
                SystemData systemData = _dataService.packAllData();
                string json = JsonConvert.SerializeObject(systemData);

                serialPort.WriteLine(json);
                EventLogger.Log("Data sent: " + json);
            }
            else
            {
                EventLogger.Log("Failed to open port.");
            }

            serialPort.Close();
        }
    }
}
