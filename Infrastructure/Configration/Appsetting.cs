using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public static class Appsetting
    {
        public static IConfiguration Configuration { get; set; }

        public static string DragonConnectionString
        {
            get
            {
                return Configuration["DragonDatabase"];
            }
        }

        //public static RabbitmqConifgModel RabbitmqConifgModel
        //{
        //    get
        //    {
        //        var rabbitmqConifgModel = new RabbitmqConifgModel();
        //        var section = Configuration.GetSection("RabbitMQConifguration");
        //        if (section != null)
        //        {
        //            rabbitmqConifgModel = section.Get<RabbitmqConifgModel>();
        //        }
        //        return rabbitmqConifgModel;
        //    }
        //}
    }
}
