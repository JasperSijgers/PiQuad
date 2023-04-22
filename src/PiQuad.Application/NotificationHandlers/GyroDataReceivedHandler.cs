using MediatR;
using Microsoft.Extensions.Logging;
using PiQuad.Contracts.Events;

namespace PiQuad.Application.NotificationHandlers;

public class GyroDataReceivedHandler : INotificationHandler<GyroDataReceivedEvent>
{
    private readonly ILogger<GyroDataReceivedHandler> _logger;

    public GyroDataReceivedHandler(ILogger<GyroDataReceivedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(GyroDataReceivedEvent request, CancellationToken cancellationToken)
    {
        var reading = request.ImuReading;
        
        _logger.LogInformation("Got gyro reading: {GyroscopeX}, {GyroscopeY}, {GyroscopeZ} (X, Y, Z)",
            reading.GyroscopeX, reading.GyroscopeY, reading.GyroscopeZ);
        _logger.LogInformation("Got accel reading: {AccelerationX}, {AccelerationY}, {AccelerationZ} (X, Y, Z)",
            reading.AccelerationX, reading.AccelerationY, reading.AccelerationZ);

        return Task.CompletedTask;
    }
}