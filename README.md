# Resource Monitor Project - server

This project is a server application that collects information about computer components, such as CPU, GPU, and RAM. The program can transmit this data via TCP/IP or UART in JSON format. It also provides a logger for event tracking and a TrayIcon manager that allows you to minimize the program to the system tray to avoid distraction, or show the console from the tray. The program also features a configuration file where users can specify data transmission paths (TCP or UART), define com-port numbers (for UART) or IP addresses (for TCP), and set data transmission intervals (for UART).

## Features

- **Data Collection**:
  - **CPU**: The program collects general information about the CPU and individual core data, including thread load, temperature, frequency, and voltage.
  - **GPU**: Information about the GPU is collected, including load, frequency, temperature, voltage, and fan speed.
  - **RAM**: Data about total memory, used memory, available memory, and virtual memory.

- **Data Transmission**:
  - Data can be transmitted via **TCP/IP** or **UART** in JSON format.
  - The program allows configuring the data transmission interval for UART or TCP/IP.

- **Logger**:
  - The logger tracks events and can output logs to the console or save them in a text file.

- **TrayIcon Manager**:
  - The program can be minimized to the system tray, allowing it to run in the background without distractions, or you can show the console window from the tray.

- **Configuration**:
  - A configuration file allows the user to specify:
    - Transmission method (TCP or UART).
    - IP addresses for TCP/IP.
    - COM-port for UART.
    - Data transmission intervals for UART.

## Installation

1. Download and extract the project.
2. Run the program using your preferred environment (e.g., via command line or an integrated development environment like visual studio).
3. Configure the configuration file according to your setup.

  **Or**

1. Download and extract the project from release.
3. Configure the configuration file according to your setup.
2. Run the program via exe.

### Sample Configuration

```json
{
  "DataReceptionMode": "UART",    // Type of transmission
  "IpAddress": "192.168.0.1",     // Local IPv4 address of the computer where the program is running
  "COM": "COM4",                  // COM port where you connect Arduino
  "Interval": 1                   // Interval in seconds
}
