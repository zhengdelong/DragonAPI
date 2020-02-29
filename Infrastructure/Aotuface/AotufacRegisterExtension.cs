using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class AotufacRegisterExtension
    {
        public static void Registe(this ContainerBuilder builder) 
        {
            //var controllerBaseType = typeof(ControllerBase);
            //builder.RegisterAssemblyTypes(typeof(Program).Assembly)
            //    .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            //    .PropertiesAutowired();
        }
    }
}
