using Microsoft.Extensions.Hosting;
using PiQuad.Application.Services;

namespace PiQuad.Infrastructure.ImuService;

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