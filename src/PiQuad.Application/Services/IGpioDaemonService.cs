namespace PiQuad.Application.Services;

public interface IGpioDaemonService
{
    Task SendMessage(byte[] message);
}