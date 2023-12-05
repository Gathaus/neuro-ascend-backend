using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Neuro.Infrastructure.MessageBus.Consumers;
using Neuro.Infrastructure.MessageBus.Messages;

namespace Neuro.Infrastructure.MessageBus.Configuration
{
    public static class MassTransitConfiguration
    {
        public static void ConfigureMassTransit(this IServiceCollection services, string rabbitMqUrl)
        {
            if (string.IsNullOrWhiteSpace(rabbitMqUrl))
                return;
            
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(rabbitMqUrl));

                    cfg.UseMessageRetry(retryConfig => retryConfig.Interval(3, TimeSpan.FromSeconds(10))); // Try 3 times, 10 seconds apart

                    ConfigureEndpoint<SampleMessageConsumer, SampleMessage>(cfg, "sample-queue",context);
                    
                });
            });

            services.AddMassTransitHostedService();
        }

        private static void ConfigureEndpoint<TConsumer, TMessage>(IRabbitMqBusFactoryConfigurator cfg, string queueName, IBusRegistrationContext context)
            where TConsumer : class, IConsumer<TMessage>
            where TMessage : class
        {
            cfg.ReceiveEndpoint(queueName, e =>
            {
                e.Consumer<TConsumer>(context);
            });
        }
    }
}