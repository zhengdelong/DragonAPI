using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class Appsetting
    {
        public static IConfiguration Configuration { get; set; }

        public static string DragonConnectionString 
        {
            get {
                return Configuration["DragonDatabase"];
            }
        }
    }
}
