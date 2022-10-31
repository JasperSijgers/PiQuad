using PiQuad.Application.MotorControllerService.Types;

namespace PiQuad.Application.MotorControllerService;

public interface IMotorControllerService
{
    void SetMotorThrottle(MotorLocation location, int throttle);
}