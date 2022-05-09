using System;
using MovieDatabase.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MovieDatabase
{
    internal class Startup
    {
        public IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddFile("app.log");
            });

            // Add new lines of code here to register any interfaces and concrete services you create
            services.AddTransient<IMainService, MainService>();
            services.AddTransient<IDataService, DataService>();

            return services.BuildServiceProvider();
        }
    }
}