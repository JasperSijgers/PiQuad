namespace PiQuad.Application.CommunicationService;

public class Message
{
    public MessageType MessageType { get; set; }
    public string[]? Arguments { get; set; } = Array.Empty<string>();
}