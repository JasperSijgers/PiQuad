using Microsoft.AspNetCore.SignalR.Client;

namespace PiQuad.Application.Services;

public interface ISignalRConnectionFactory
{
    HubConnection GetConnection();
}