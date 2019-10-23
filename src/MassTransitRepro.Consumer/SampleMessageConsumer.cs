using System.Threading.Tasks;
using MassTransit;
using MassTransitRepro.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransitRepro.Publisher
{
    class SampleMessageConsumer : IConsumer<SampleMessage>
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger _logger;

        public SampleMessageConsumer(
            IHostEnvironment hostEnvironment,
            ILogger logger)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SampleMessage> context)
        {
            _logger.LogInformation("Consumed!", _hostEnvironment.EnvironmentName, context.Message);
        }
    }
}