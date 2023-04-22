namespace PiQuad.Infrastructure.ImuService.Settings;

public class ImuSettings
{
    public int BusId { get; set; } = 1;
    public int InterruptPin { get; set; } = 18;
    public byte Address { get; set; } = 0x68;
}