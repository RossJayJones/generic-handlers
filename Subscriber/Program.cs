using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.PubSub.Subscriber";
        var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.Subscriber");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.DisableFeature<TimeoutManager>();

        transport.Routing().RegisterPublisher(typeof(IHaveData), "Samples.PubSub.Publisher");

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}