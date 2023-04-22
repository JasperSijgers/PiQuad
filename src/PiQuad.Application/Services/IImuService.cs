using PiQuad.Contracts.Types;

namespace PiQuad.Application.Services;

public interface IImuService
{
    void AddEventHandler(EventHandler<ImuReading> handler);
    Task Start();
    void Stop();
}