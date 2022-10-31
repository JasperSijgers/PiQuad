using PiQuad.Application.GpioDaemonService;
using PiQuad.Application.MotorControllerService.Exceptions;

namespace PiQuad.Application.MotorControllerService.Types;

public class Motor
{
    private readonly IGpioDaemonService _gpioDaemonService;

    public int Pin { get; set; }
    public int MinThrottle { get; set; }
    public int MaxThrottle { get; set; }
    private int _throttle { get; set; }

    public int Throttle
    {
        get => _throttle;
        set
        {
            if (value > MaxThrottle || value < MinThrottle)
            {
                throw new MotorSpeedOutOfRangeException(
                    $"Throttle value must be within range [{MinThrottle} ... {MaxThrottle}]");
            }

            _throttle = value;
            UpdateMotorThrottle();
        }
    }

    public Motor(IGpioDaemonService gpioDaemonService)
    {
        _gpioDaemonService = gpioDaemonService;
    }

    private void UpdateMotorThrottle()
    {
        var throttleCommand = new int[] {8, Pin, _throttle, 0};
        var message = new byte[16];
        Buffer.BlockCopy(throttleCommand, 0, message, 0, message.Length);

        _gpioDaemonService.SendMessage(message);
    }
}