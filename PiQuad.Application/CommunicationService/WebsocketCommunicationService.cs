using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PiQuad.Application.CommunicationService.Settings;

namespace PiQuad.Application.CommunicationService;

public class WebsocketCommunicationService : ICommunicationService, IDisposable
{
    private readonly WebsocketSettings _settings;
    private readonly ILogger<WebsocketCommunicationService> _logger;

    private readonly TcpListener _server;
    private TcpClient? _client;
    private NetworkStream? _stream;

    private DateTime _lastMessageReceivedAt;
    public event EventHandler<Message>? MessageReceivedEvent;

    public WebsocketCommunicationService(IOptions<WebsocketSettings> settings,
        ILogger<WebsocketCommunicationService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        _server = new TcpListener(IPAddress.Parse(_settings.IpAddress), _settings.Port);
    }

    public void Start(CancellationToken cancellationToken)
    {
        _server.Start();
        _logger.LogInformation("Started the websocket at {Ip}:{Port}", _settings.IpAddress, _settings.Port);

        while (!cancellationToken.IsCancellationRequested)
        {
            if (_client is null || !_client.Connected)
            {
                _logger.LogInformation("Waiting for a client to connect...");
                AwaitClient();
                _logger.LogInformation("Client connected!");
                _lastMessageReceivedAt = DateTime.MinValue;
            }

            AwaitIncomingMessage();
        }
    }

    public void AddEventHandler(EventHandler<Message> handler)
    {
        MessageReceivedEvent += handler;
    }

    private void AwaitClient()
    {
        _client = _server.AcceptTcpClient();
        _stream = _client.GetStream();
    }

    private void DisconnectClient()
    {
        _stream?.Close();
        _client?.Close();
    }

    private bool ClientHasExceededTimeout()
    {
        if (!_client.Connected || _lastMessageReceivedAt == DateTime.MinValue)
        {
            return false;
        }

        return (DateTime.Now - _lastMessageReceivedAt) > TimeSpan.FromMilliseconds(_settings.TimeoutInMilliseconds);
    }

    private void AwaitIncomingMessage()
    {
        while (_client.Connected)
        {
            if (ClientHasExceededTimeout())
            {
                _logger.LogInformation("Connection to client lost, disconnecting...");
                DisconnectClient();
                return;
            }

            if (_stream.DataAvailable)
            {
                HandleIncomingMessage();
            }
        }
    }

    private void HandleIncomingMessage()
    {
        _lastMessageReceivedAt = DateTime.Now;

        var bytes = new Byte[_client.Available];
        _stream.Read(bytes, 0, bytes.Length);

        var messageType = ParseMessageType(bytes);
        var args = ParseMessageArgs(bytes);

        if (messageType is not null && args is not null)
        {
            HandleMessage(messageType.Value, args);
        }
    }

    private void HandleMessage(MessageType messageType, string[] args)
    {
        switch (messageType)
        {
            case MessageType.Throttle:
                PublishEvent(messageType, args);
                break;
            case MessageType.Disconnect:
                DisconnectClient();
                break;
        }

        _logger.LogInformation("Message from client received: {Type} ({Args})", messageType, string.Join(" ", args));
    }

    private void PublishEvent(MessageType messageType, string[] args)
    {
        if (MessageReceivedEvent is null)
        {
            return;
        }

        MessageReceivedEvent(this, new Message()
        {
            MessageType = messageType,
            Arguments = args
        });
    }

    private MessageType? ParseMessageType(byte[] message)
    {
        var msgString = Encoding.UTF8.GetString(message);
        return Enum.Parse<MessageType>(msgString.Split(" ")[0]);
    }

    private string[]? ParseMessageArgs(byte[] message)
    {
        var arr = Encoding.UTF8.GetString(message).Split(" ");

        if (arr.Length < 2)
        {
            return null;
        }

        return arr.Skip(1).ToArray();
    }

    public void Stop()
    {
        Dispose();
    }

    public void Dispose()
    {
        _stream?.Dispose();
        _client?.Dispose();
    }
}