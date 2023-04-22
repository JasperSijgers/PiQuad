using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PiQuad.Application;
using PiQuad.Application.Services;
using PiQuad.Contracts.Requests;

namespace PiQuad.Infrastructure.SignalR;

public class SignalRHostedService : IHostedService
{
    private readonly ISignalRConnectionFactory _connectionFactory;
    private readonly IMediator _mediator;
    private readonly ILogger<SignalRHostedService> _logger;

    private HubConnection? _connection;

    public SignalRHostedService(ISignalRConnectionFactory connectionFactory, IMediator mediator, ILogger<SignalRHostedService> logger)
    {
        _connectionFactory = connectionFactory;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _connection = _connectionFactory.GetConnection();

        _connection.Closed += OnConnectionClosed;
        _connection.Reconnecting += OnConnectionReconnecting;
        _connection.Reconnected += OnConnectionReconnected;

        _connection.On<IRequest>("Control", request =>
        {
            _mediator.Publish(request, cancellationToken);
        });

        while (_connection.State != HubConnectionState.Connected)
        {
            _logger.LogInformation("Attempting to connect to SignalR hub...");
            await _connection.StartAsync(cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
        
        _logger.LogInformation("Connection to SignalR hub established with connection id: {ConnectionId}", _connection.ConnectionId);
    }

    private Task OnConnectionReconnecting(Exception? error)
    {
        if (error == null)
        {
            _logger.LogError("Connection to SignalR hub failed");
        }
        else
        {
            _logger.LogError("Connection to SignalR hub failed with error: {Error}", error);
        }

        return Task.CompletedTask;
    }
    
    private Task OnConnectionClosed(Exception? error)
    {
        if (error == null)
        {
            _logger.LogError("Connection to SignalR hub closed");
        }
        else
        {
            _logger.LogError("Connection to SignalR hub closed with error: {Error}", error);
        }
        
        return Task.CompletedTask;
    }

    private Task OnConnectionReconnected(string? connectionId)
    {
        _logger.LogInformation("Connection to SignalR hub re-established with connection id: {ConnectionId}", connectionId);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_connection is null)
        {
            return;
        }

        await _connection.StopAsync(cancellationToken);
    }
}