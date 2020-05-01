using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.RabbitMQ
{
    public class RabbitmqConifgModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public short Port { get; set; }
    }
}
