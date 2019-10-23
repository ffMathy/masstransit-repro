using System;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using MassTransit;
using MassTransit.AmazonSqsTransport;
using MassTransit.AmazonSqsTransport.Configuration;
using MassTransit.AmazonSqsTransport.Configuration.Configurators;
using MassTransit.Definition;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace MassTransitRepro.Shared
{
    public class IocSetup
    {
        public static void Register(
            IServiceCollection services,
            string environmentName,
            Action<IServiceCollectionConfigurator> additionalRegistrations)
        {
            services.AddSingleton<IHostEnvironment>(new HostingEnvironment()
            {
                EnvironmentName = environmentName
            });

            services.AddMassTransit(configurator =>
            {
                additionalRegistrations?.Invoke(configurator);

                configurator.AddBus(provider => Bus.Factory.CreateUsingAmazonSqs(b =>
                {
                    b.Host(
                        new ConfigurationHostSettings()
                        {
                            Region = RegionEndpoint.EUWest1,
                            AmazonSnsConfig = new AmazonSimpleNotificationServiceConfig()
                            {
                                ServiceURL = "http://localhost:4575"
                            },
                            AmazonSqsConfig = new AmazonSQSConfig()
                            {
                                ServiceURL = "http://localhost:4576"
                            },
                            Credentials = new BasicAWSCredentials("foo", "bar")
                        });

                    b.Durable = true;
                    b.AutoDelete = false;
                    b.PurgeOnStartup = false;

                    b.ConfigureEndpoints(provider, new EnvironmentNameEndpointFormatter(environmentName));
                }));
            });
        }
    }

    public class EnvironmentNameEndpointFormatter : KebabCaseEndpointNameFormatter
    {
        private readonly string _environmentName;

        public EnvironmentNameEndpointFormatter(string environmentName)
        {
            _environmentName = environmentName;
        }

        protected override string SanitizeName(string name)
        {
            name = _environmentName + "-" + name;

            return base.SanitizeName(name);
        }
    }
}
