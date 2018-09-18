using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using Shared.Core;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.PubSub.Publisher";
        var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.Publisher");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.DisableFeature<TimeoutManager>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await Start(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press '1' to publish the OrderReceived event");
        Console.WriteLine("Press any other key to exit");

        #region PublishLoop

        int count = 0;
        while (true)
        {
            count++;
            var key = Console.ReadKey();
            Console.WriteLine();

            var orderReceivedId = Guid.NewGuid();
            if (key.Key == ConsoleKey.D1)
            {
                if (count % 2 == 0)
                {
                    await endpointInstance.Publish(new OrderReceived<SomeOtherData>
                    {
                        OrderId = orderReceivedId,
                        Data = new SomeOtherData { Id = "10", Number = count }
                    }).ConfigureAwait(false);
                    Console.WriteLine($"Published OrderReceived with some other data Event with Id {orderReceivedId}.");
                }
                else
                {
                    await endpointInstance.Publish(new OrderReceived<SomeData>
                    {
                        OrderId = orderReceivedId,
                        Data = new SomeData { Id = "1" }
                    }).ConfigureAwait(false);
                    Console.WriteLine($"Published OrderReceived Event with Id {orderReceivedId}.");
                }
            }
            else
            {
                return;
            }
        }

        #endregion
    }
}