using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

internal class OrderReceivedHandler<T> : IHandleMessages<OrderReceived<T>>
{
    static ILog log = LogManager.GetLogger<OrderReceivedHandler<T>>();

    public Task Handle(OrderReceived<T> message, IMessageHandlerContext context)
    {
        log.Info($"Subscriber has received OrderReceived event with OrderId {message.OrderId}.");
        return Task.CompletedTask;
    }
}