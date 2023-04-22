using PiQuad.Contracts.Types;

namespace PiQuad.Application.Services;

public interface IImu
{
    Task Initialize();
    void Stop();
    void AddEventHandler(EventHandler<ImuReading> handler);
}