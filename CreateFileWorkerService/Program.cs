using CreateFileWorkerService.Models;
using CreateFileWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateFileWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    IConfiguration configuration = hostContext.Configuration;
                    services.AddDbContext<NorthwindContext>(o =>
                    {
                        o.UseSqlServer(configuration.GetConnectionString("SqlServer"));
                    });
                    services.AddSingleton<RabbitMQClientService>();
                    services.AddSingleton(sp => new ConnectionFactory()
                    {
                        Uri = new Uri(configuration.GetConnectionString("RabbitMQ")),
                        DispatchConsumersAsync = true
                    });
                    services.AddHostedService<Worker>();
                });
    }
}
