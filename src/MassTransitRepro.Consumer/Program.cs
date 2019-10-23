using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitRepro.Publisher;
using MassTransitRepro.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace MassTransitRepro.Consumer
{
    class Program
    {
        private readonly IBusControl _busControl;

        public Program(
            IBusControl busControl)
        {
            _busControl = busControl;
        }

        private async Task RunAsync()
        {
            await _busControl.StartAsync();

            while (true)
            {
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
                additionalRegistrations: configurator =>
                {
                    configurator.AddConsumer<SampleMessageConsumer>();
                });

            var serviceProvider = services.BuildServiceProvider();

            var program = serviceProvider.GetRequiredService<Program>();
            await program.RunAsync();
        }
    }
}
