using Microsoft.Extensions.Hosting;

namespace PiQuad.Application.PiQuadHostedService;

public class PiQuadHostedService : IHostedService
{
    private readonly PiQuadControllerService _controller;

    public PiQuadHostedService(PiQuadControllerService controller)
    {
        _controller = controller;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _controller.Start();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}