using Microsoft.Extensions.Hosting;
using PiQuad.Application.CommunicationService;
using PiQuad.Application.ImuService;

namespace PiQuad.Application.PiQuadHostedService;

public class ImuHostedService : IHostedService
{
    private readonly IImuService _imuService;

    public ImuHostedService(IImuService imuService)
    {
        _imuService = imuService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _imuService.Start();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _imuService.Stop();
        return Task.CompletedTask;
    }
}