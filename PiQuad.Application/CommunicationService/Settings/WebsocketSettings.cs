namespace PiQuad.Application.CommunicationService.Settings;

public class WebsocketSettings
{
    public string IpAddress { get; set; } = "";
    public int Port { get; set; }
    public int TimeoutInMilliseconds { get; set; }
}