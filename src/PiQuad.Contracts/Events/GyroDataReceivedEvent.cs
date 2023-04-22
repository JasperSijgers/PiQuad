using MediatR;
using PiQuad.Contracts.Types;

namespace PiQuad.Contracts.Events;

public class GyroDataReceivedEvent : INotification
{
    public ImuReading ImuReading { get; set; } = new();
}