using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Bus
{
    public class BusOptions
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 9094;
        public string BootstrapServers => $"{Host}:{Port}";
    }
}
