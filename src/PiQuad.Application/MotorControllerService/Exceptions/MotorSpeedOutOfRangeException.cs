namespace PiQuad.Application.MotorControllerService.Exceptions;

public class MotorSpeedOutOfRangeException : Exception
{
    public MotorSpeedOutOfRangeException()
    {
    }

    public MotorSpeedOutOfRangeException(string message) : base(message)
    {
    }

    public MotorSpeedOutOfRangeException(string message, Exception inner) : base(message, inner)
    {
    }
}