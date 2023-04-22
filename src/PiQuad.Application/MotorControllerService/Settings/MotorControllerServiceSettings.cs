namespace PiQuad.Application.MotorControllerService.Settings;

public class MotorControllerServiceSettings
{
    public int[] GpioPins { get; set; } = Array.Empty<int>();

    public int MinThrottle { get; set; }

    public int MaxThrottle { get; set; }
}