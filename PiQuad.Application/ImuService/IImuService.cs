using PiQuad.Application.ImuService.Types;

namespace PiQuad.Application.ImuService;

public interface IImuService
{
    void AddEventHandler(EventHandler<ImuReading> handler);
    Task Start();
    void Stop();
}