using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using PiQuad.Application.Services;
using PiQuad.Application.SignalR;

namespace PiQuad.Infrastructure.SignalR;

public class SignalRConnectionFactory : ISignalRConnectionFactory
{
    private readonly SignalRSettings _settings;
    
    private HubConnection? _connection;

    public SignalRConnectionFactory(IOptions<SignalRSettings> settings)
    {
        _settings = settings.Value;
    }

    public HubConnection GetConnection()
    {
        if (_connection is null)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(_settings.Url)
                .WithAutomaticReconnect()
                .Build();
        }

        return _connection;
    }
}