namespace PiQuad.Application.MotorControllerService.Settings;

public class MotorControllerServiceSettings
{
    public int[] GpioPins { get; set; }

    public int MinThrottle { get; set; }

    public int MaxThrottle { get; set; }
}