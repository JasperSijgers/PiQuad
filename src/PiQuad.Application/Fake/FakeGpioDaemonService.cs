using PiQuad.Application.Services;

namespace PiQuad.Application.Fake;

public class FakeGpioDaemonService : IGpioDaemonService
{
    public Task SendMessage(byte[] message)
    {
        return Task.CompletedTask;
    }
}