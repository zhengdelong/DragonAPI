using Microsoft.Extensions.Configuration;

namespace Infrastructure.RabbitMQ
{
    public static class RabbitMQSetting
    {
        public static IConfiguration Configuration { get; set; }
        public static RabbitmqConifgModel RabbitmqConifgModel
        {
            get
            {
                var rabbitmqConifgModel = new RabbitmqConifgModel();
                var section = Configuration.GetSection("RabbitMQConifguration");
                if (section != null)
                {
                    rabbitmqConifgModel = section.Get<RabbitmqConifgModel>();
                }
                return rabbitmqConifgModel;
            }
        }
    }
}
