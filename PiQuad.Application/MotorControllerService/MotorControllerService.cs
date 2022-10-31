using PiQuad.Application.GpioDaemonService;
using PiQuad.Application.MotorControllerService.Settings;
using PiQuad.Application.MotorControllerService.Types;

namespace PiQuad.Application.MotorControllerService;

public class MotorControllerService : IMotorControllerService
{
    private readonly MotorControllerServiceSettings _settings;
    private readonly IGpioDaemonService _gpioDaemonService;
    private readonly Dictionary<MotorLocation, Motor> _motors;

    public MotorControllerService(MotorControllerServiceSettings settings, IGpioDaemonService gpioDaemonService)
    {
        _settings = settings;
        _gpioDaemonService = gpioDaemonService;

        _motors = new Dictionary<MotorLocation, Motor>
        {
            {MotorLocation.BackRight, CreateMotor(_settings.GpioPins[0])},
            {MotorLocation.FrontRight, CreateMotor(_settings.GpioPins[1])},
            {MotorLocation.BackLeft, CreateMotor(_settings.GpioPins[2])},
            {MotorLocation.FrontLeft, CreateMotor(_settings.GpioPins[3])}
        };
    }

    private Motor CreateMotor(int pin)
    {
        return new Motor(_gpioDaemonService)
        {
            Pin = pin,
            MinThrottle = _settings.MinThrottle,
            MaxThrottle = _settings.MaxThrottle
        };
    }

    public void SetMotorThrottle(MotorLocation location, int throttle)
    {
        _motors[location].Throttle = throttle;
    }
}