using Microsoft.Extensions.Logging;
using PiQuad.Application.ImuService;
using PiQuad.Application.ImuService.Types;
using PiQuad.Application.MotorControllerService;

namespace PiQuad.Application.PiQuadHostedService;

public class PiQuadControllerService
{
    private readonly IMotorControllerService _motorService;
    private readonly IImuService _imuService;
    private readonly ILogger<PiQuadControllerService> _logger;

    public PiQuadControllerService(IMotorControllerService motorService, IImuService imuService,
        ILogger<PiQuadControllerService> logger)
    {
        _motorService = motorService;
        _imuService = imuService;
        _logger = logger;

        _imuService.AddEventHandler(AdjustThrottleForGyroData);
    }

    public async Task Start()
    {
        await _imuService.Start();
    }

    private void AdjustThrottleForGyroData(object? sender, ImuReading reading)
    {
        _logger.LogInformation("Got gyro reading: {GyroscopeX}, {GyroscopeY}, {GyroscopeZ} (X, Y, Z)",
            reading.GyroscopeX, reading.GyroscopeY, reading.GyroscopeZ);
        _logger.LogInformation("Got accel reading: {AccelerationX}, {AccelerationY}, {AccelerationZ} (X, Y, Z)",
            reading.AccelerationX, reading.AccelerationY, reading.AccelerationZ);

        // TODO: Do some calculations using reading

        // TODO: Set the motor throttles
    }
}