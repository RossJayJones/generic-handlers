using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared.Core;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.PubSub.Publisher";
        var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.Publisher");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

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

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var orderReceivedId = Guid.NewGuid();
            if (key.Key == ConsoleKey.D1)
            {
                var orderReceived = new OrderReceived<SomeData>
                {
                    OrderId = orderReceivedId,
                    Data = new SomeData {  Id = "1"}
                };
                await endpointInstance.Publish(orderReceived)
                    .ConfigureAwait(false);
                Console.WriteLine($"Published OrderReceived Event with Id {orderReceivedId}.");
            }
            else
            {
                return;
            }
        }

        #endregion
    }
}