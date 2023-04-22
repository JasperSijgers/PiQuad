using MediatR;
using PiQuad.Application.Services;
using PiQuad.Contracts.Events;
using PiQuad.Contracts.Types;

namespace PiQuad.Infrastructure.ImuService;

public class ImuService : IImuService
{
    private readonly IImu _imu;
    private readonly IMediator _mediator;

    public ImuService(IImu imu, IMediator mediator)
    {
        _imu = imu;
        _mediator = mediator;
    }

    public void AddEventHandler(EventHandler<ImuReading> handler)
    {
        _imu.AddEventHandler(handler);
    }

    public async Task Start()
    {
        _imu.AddEventHandler(ReceiveGyroData);
        await _imu.Initialize();
    }

    private void ReceiveGyroData(object? sender, ImuReading reading)
    {
        _mediator.Publish(new GyroDataReceivedEvent()
        {
            ImuReading = reading
        });
    }

    public void Stop()
    {
        _imu.Stop();
    }
}