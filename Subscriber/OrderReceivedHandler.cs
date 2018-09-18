using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using ReflectionMagic;
using Shared.Core;

internal class OrderReceivedHandler : IHandleMessages<IHaveData>
{
    static ILog log = LogManager.GetLogger<OrderReceivedHandler>();

    public Task Handle(IHaveData message, IMessageHandlerContext context)
    {
        var data = message.AsDynamic().Data;

        this.AsDynamic().Handle(data);

        return Task.CompletedTask;
    }

    void Handle(SomeData data)
    {
        log.Info($"Some data came through with Id {data.Id}");
    }

    void Handle(SomeOtherData data)
    {
        log.Info($"Some other data came through with Id {data.Id} and number {data.Number}");
    }
}