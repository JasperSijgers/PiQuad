using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PiQuad.Application.Services;
using PiQuad.Infrastructure.GpioDaemonService.Settings;

namespace PiQuad.Infrastructure.GpioDaemonService;

public class GpioDaemonService : IGpioDaemonService, IDisposable
{
    private readonly GpioDaemonServiceSettings _settings;
    private readonly ILogger<GpioDaemonService> _logger;
    private Socket? _client;

    public GpioDaemonService(IOptions<GpioDaemonServiceSettings> settings, ILogger<GpioDaemonService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        ConnectToDaemon();
    }

    private void ConnectToDaemon()
    {
        try
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(_settings.IpAddress), _settings.Port);
            _client = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _client.Connect(endpoint);
            _logger.LogInformation("Connected to GPIO daemon at {Ip}:{Port}", _settings.IpAddress, _settings.Port);
        }
        catch (SocketException e)
        {
            _logger.LogError("Could not connect to GPIO daemon: {Message}", e.Message);
        }
    }

    public async Task SendMessage(byte[] message)
    {
        try
        {
            if (_client is null || !_client.Connected)
            {
                _logger.LogError("Could not send message to GPIO daemon: daemon uninitialized or disconnected!");
                return;
            }

            await _client.SendAsync(message, SocketFlags.None);
            _logger.LogInformation("Sent message to GPIO daemon: {Message}",
                System.Text.Encoding.Default.GetString(message));
        }
        catch (SocketException e)
        {
            _logger.LogError("Could not send message to GPIO daemon: {Message}", e.Message);
        }
    }

    public void Dispose()
    {
        if (_client is null)
        {
            return;
        }

        if (_client.Connected)
        {
            _client.Disconnect(false);
        }

        _client.Dispose();
    }
}