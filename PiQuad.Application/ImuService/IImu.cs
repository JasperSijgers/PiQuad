using PiQuad.Application.ImuService.Types;

namespace PiQuad.Application.ImuService;

public interface IImu
{
    Task Initialize();
    void Stop();
    void AddEventHandler(EventHandler<ImuReading> handler);
}