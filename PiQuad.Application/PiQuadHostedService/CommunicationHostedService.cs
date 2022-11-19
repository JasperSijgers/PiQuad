using Microsoft.Extensions.Hosting;
using PiQuad.Application.CommunicationService;

namespace PiQuad.Application.PiQuadHostedService;

public class CommunicationHostedService : IHostedService
{
    private readonly ICommunicationService _communicationService;

    public CommunicationHostedService(PiQuadControllerService controller, ICommunicationService communicationService)
    {
        _communicationService = communicationService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _communicationService.Start(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _communicationService.Stop();
        return Task.CompletedTask;
    }
}