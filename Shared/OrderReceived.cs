using System;
using NServiceBus;
using Shared.Core;

public class OrderReceived<T> :
    IHaveData
{
    public Guid OrderId { get; set; }

    public T Data { get; set; }
}

public interface IHaveData : IEvent { }