using Autofac;
using Infrastructure;
using System.Reflection;
using System.Linq;
using MySql.Data.MySqlClient;
using Infrastructure.RabbitMQ;

namespace DragonAPI
{
    public static class AotufacRegisterExtension
    {
        /// <summary>
        /// Ioc注册
        /// </summary>
        /// <param name="builder"></param>
        public static void Registe(this ContainerBuilder builder)
        {
            //var controllerBaseType = typeof(ControllerBase);
            //builder.RegisterAssemblyTypes(typeof(Program).Assembly)
            //    .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            //    .PropertiesAutowired();

            var repositoriesType = GetAssemblies("Dapper.Repositories");

            builder.RegisterAssemblyTypes(repositoriesType)
                   .Where(t => typeof(IBaseRepository).GetTypeInfo().IsAssignableFrom(t))
                   .AsImplementedInterfaces();

            var servicesType = GetAssemblies("Services");
            builder.RegisterAssemblyTypes(servicesType)
                   .Where(t => typeof(IService).GetTypeInfo().IsAssignableFrom(t))
                   .AsImplementedInterfaces();
            builder.Register(c => new MySqlConnection(Appsetting.DragonConnectionString));
            builder.RegisterType(typeof(RabbitMQClient)).SingleInstance();
            builder.RegisterType(typeof(RabbitMQProvidor)).As(typeof(IRabbitMQProvidor)).SingleInstance();
            //builder.RegisterType<DragonDBContext>();
        }
        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <param name="virtualPaths"></param>
        /// <returns></returns>
        private static Assembly GetAssemblies(string virtualPaths)
        {
            if (!string.IsNullOrEmpty(virtualPaths))
            {
                return Assembly.Load(virtualPaths);
            }
            else
            {
                return null;
            }
        }
    }
}
