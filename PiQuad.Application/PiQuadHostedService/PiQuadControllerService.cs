using Microsoft.Extensions.Logging;
using PiQuad.Application.CommunicationService;
using PiQuad.Application.ImuService;
using PiQuad.Application.ImuService.Types;
using PiQuad.Application.MotorControllerService;
using PiQuad.Application.MotorControllerService.Types;

namespace PiQuad.Application.PiQuadHostedService;

public class PiQuadControllerService
{
    private readonly IMotorControllerService _motorService;
    private readonly IImuService _imuService;
    private readonly ICommunicationService _communicationService;
    private readonly ILogger<PiQuadControllerService> _logger;

    private int _requestedThrottle = 1000;
    private ImuReading _imuReading = new ImuReading();

    public PiQuadControllerService(IMotorControllerService motorService, IImuService imuService,
        ICommunicationService communicationService, ILogger<PiQuadControllerService> logger)
    {
        _motorService = motorService;
        _imuService = imuService;
        _communicationService = communicationService;
        _logger = logger;

        _imuService.AddEventHandler(ReceiveGyroData);
        _communicationService.AddEventHandler(ReceiveCommunication);
    }

    private void UpdateMotorSpeeds()
    {
        // TODO: Calculate motor speeds

        _motorService.SetMotorThrottle(MotorLocation.BackRight, _requestedThrottle);
        _motorService.SetMotorThrottle(MotorLocation.FrontRight, _requestedThrottle);
        _motorService.SetMotorThrottle(MotorLocation.BackLeft, _requestedThrottle);
        _motorService.SetMotorThrottle(MotorLocation.FrontLeft, _requestedThrottle);
        
        _logger.LogInformation("Updated motor throttle: {Throttle}", _requestedThrottle);
    }

    private void ReceiveCommunication(object? sender, Message? message)
    {
        _logger.LogInformation("Received motor communication message: {Args}", message?.Arguments?[0]);
        if (message?.MessageType == MessageType.Throttle && message.Arguments is not null)
        {
            _requestedThrottle = int.Parse(message.Arguments[0]);
            UpdateMotorSpeeds();
        }
    }

    private void ReceiveGyroData(object? sender, ImuReading reading)
    {
        _logger.LogInformation("Got gyro reading: {GyroscopeX}, {GyroscopeY}, {GyroscopeZ} (X, Y, Z)",
            reading.GyroscopeX, reading.GyroscopeY, reading.GyroscopeZ);
        _logger.LogInformation("Got accel reading: {AccelerationX}, {AccelerationY}, {AccelerationZ} (X, Y, Z)",
            reading.AccelerationX, reading.AccelerationY, reading.AccelerationZ);

        _imuReading = reading;
        UpdateMotorSpeeds();
    }
}