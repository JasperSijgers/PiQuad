namespace PiQuad.Application.GpioDaemonService;

public interface IGpioDaemonService
{
    Task SendMessage(byte[] message);

    Task<byte[]?> ReceiveAsync(int size);
}