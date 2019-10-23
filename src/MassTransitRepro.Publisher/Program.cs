using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitRepro.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;

namespace MassTransitRepro.Publisher
{
    class Program
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public Program(
            IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        private async Task RunAsync()
        {
            while (true)
            {
                await _publishEndpoint.Publish(new SampleMessage()
                {
                    Foo = "bar",
                    Time = DateTime.Now
                });

                await Task.Delay(1000);
            }
        }

        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddScoped<Program>();

            IocSetup.Register(
                services,
                environmentName: args[0],
                additionalRegistrations: null);

            var serviceProvider = services.BuildServiceProvider();

            var program = serviceProvider.GetRequiredService<Program>();
            await program.RunAsync();
        }
    }
}
