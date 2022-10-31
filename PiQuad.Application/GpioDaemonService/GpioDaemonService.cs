using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using PiQuad.Application.GpioDaemonService.Settings;

namespace PiQuad.Application.GpioDaemonService;

public class GpioDaemonService : IGpioDaemonService, IDisposable
{
    private GpioDaemonServiceSettings _settings;
    private ILogger<GpioDaemonService> _logger;
    private Socket? _client;

    public GpioDaemonService(GpioDaemonServiceSettings settings, ILogger<GpioDaemonService> logger)
    {
        _settings = settings;
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
            await _client.SendAsync(message, SocketFlags.None);
        }
        catch (SocketException e)
        {
            _logger.LogError("Could not send message to GPIO daemon: {Message}", e.Message);
        }
    }

    public async Task<byte[]?> ReceiveAsync(int size)
    {
        try
        {
            var buffer = new byte[size];
            await _client.ReceiveAsync(buffer, SocketFlags.None);
            return buffer;
        }
        catch (SocketException e)
        {
            _logger.LogError("Could not receive message from GPIO daemon: {Message}", e.Message);
        }

        return null;
    }

    public void Dispose()
    {
        if (_client.Connected)
        {
            _client.Disconnect(false);
        }

        _client.Dispose();
    }
}