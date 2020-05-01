using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;

namespace Infrastructure.RabbitMQ
{
    public class RabbitMQClient
    {
        protected static IConnection _connection;
        private static readonly object _Lock = new object();

        public RabbitMQClient()
        {
            if (_connection == null)
            {
                lock (_Lock)
                {
                    if (_connection == null)
                    {
                        ConnectionFactory factory = new ConnectionFactory();
                        var configuration = RabbitMQSetting.RabbitmqConifgModel;
                        // "guest"/"guest" by default, limited to localhost connections
                        factory.UserName = configuration.UserName;
                        factory.Password = configuration.Password;
                        factory.VirtualHost = configuration.VirtualHost;
                        factory.HostName = configuration.HostName;
                        factory.Port = configuration.Port;
                        factory.AutomaticRecoveryEnabled = true;

                        try
                        {
                            _connection = factory.CreateConnection();
                        }
                        catch (BrokerUnreachableException e)
                        {

                            throw;
                        }

                    }
                }
            }
        }
    }
}
