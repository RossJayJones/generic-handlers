using System;
using NServiceBus;

public class OrderReceived<T> :
    IEvent
{
    public Guid OrderId { get; set; }

    public T Data { get; set; }
}