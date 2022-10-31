using PiQuad.Application.ImuService.Types;

namespace PiQuad.Application.ImuService;

public interface IImu
{
    Task Initialize();
    void AddEventHandler(EventHandler<ImuReading> handler);
}