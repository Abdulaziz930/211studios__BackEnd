using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Interfaces;

namespace Utils
{
    public static class Extensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services) 
            => services.AddScoped<ILoggerManager, LoggerManager>();

    }
}
