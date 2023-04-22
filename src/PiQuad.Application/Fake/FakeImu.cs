using PiQuad.Application.Services;
using PiQuad.Contracts.Types;

namespace PiQuad.Application.Fake;

public class FakeImu : IImu
{
    private readonly CancellationTokenSource _source;
    private readonly CancellationToken _cancellationToken;
    
    public event EventHandler<ImuReading>? SensorDataRetrievedEvent;

    public FakeImu()
    {
         _source = new CancellationTokenSource();
         _cancellationToken = _source.Token;
    }

    public async Task Initialize()
    {
        await Task.Run(async () =>
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), _cancellationToken);

                if (SensorDataRetrievedEvent is null)
                {
                    continue;
                }

                SensorDataRetrievedEvent(this, new ImuReading());
                await Task.Delay(TimeSpan.FromMilliseconds(100), _cancellationToken);
            }
        }, _cancellationToken);
    }

    public void AddEventHandler(EventHandler<ImuReading> handler)
    {
        SensorDataRetrievedEvent += handler;
    }

    public void Stop()
    {
        _source.Cancel();
    }
}