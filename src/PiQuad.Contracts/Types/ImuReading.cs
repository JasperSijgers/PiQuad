namespace PiQuad.Contracts.Types;

public class ImuReading : ITelemetryData
{
    public float AccelerationX { get; set; }
    public float AccelerationY { get; set; }
    public float AccelerationZ { get; set; }
    public float GyroscopeX { get; set; }
    public float GyroscopeY { get; set; }
    public float GyroscopeZ { get; set; }
}