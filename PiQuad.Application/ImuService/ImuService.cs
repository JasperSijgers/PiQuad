using PiQuad.Application.ImuService.Types;

namespace PiQuad.Application.ImuService;

public class ImuService : IImuService
{
    private readonly IImu _imu;

    public ImuService(IImu imu)
    {
        _imu = imu;
    }

    public void AddEventHandler(EventHandler<ImuReading> handler)
    {
        _imu.AddEventHandler(handler);
    }

    public async Task Start()
    {
        await _imu.Initialize();
    }

    public void Stop()
    {
        _imu.Stop();
    }
}