using MediatR;

namespace PiQuad.Contracts.Requests;

// ReSharper disable once InconsistentNaming
public interface ThrottleRequest : IRequest
{
    int Throttle { get; set; }
}