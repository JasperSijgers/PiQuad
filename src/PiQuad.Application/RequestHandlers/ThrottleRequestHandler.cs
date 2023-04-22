using MediatR;
using Microsoft.Extensions.Logging;
using PiQuad.Contracts.Requests;

namespace PiQuad.Application.RequestHandlers;

public class ThrottleRequestHandler : IRequestHandler<ThrottleRequest>
{
    private readonly ILogger<ThrottleRequestHandler> _logger;

    public ThrottleRequestHandler(ILogger<ThrottleRequestHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ThrottleRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Got throttle request: {Throttle}", request.Throttle);
        return Task.CompletedTask;
    }
}