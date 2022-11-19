namespace PiQuad.Application.CommunicationService;

public interface ICommunicationService
{
    void Start(CancellationToken cancellationToken);

    void Stop();

    void AddEventHandler(EventHandler<Message> handler);
}