namespace PiQuad.Infrastructure.GpioDaemonService.Settings;

public interface GpioDaemonServiceSettings
{
    public string IpAddress { get; set; }
    public int Port { get; set; }
}