using System.Device.Gpio;
using System.Device.I2c;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PiQuad.Application.Services;
using PiQuad.Contracts.Types;
using PiQuad.Infrastructure.ImuService.Settings;

namespace PiQuad.Infrastructure.ImuService;

public class Mpu6050 : IImu, IDisposable
{
    private const byte PWR_MGMT_1 = 0x6B;
    private const byte SMPLRT_DIV = 0x19;
    private const byte CONFIG = 0x1A;
    private const byte GYRO_CONFIG = 0x1B;
    private const byte ACCEL_CONFIG = 0x1C;
    private const byte FIFO_EN = 0x23;
    private const byte INT_ENABLE = 0x38;
    private const byte INT_STATUS = 0x3A;
    private const byte USER_CTRL = 0x6A;
    private const byte FIFO_COUNT = 0x72;
    private const byte FIFO_R_W = 0x74;
    private const int SensorBytes = 12;

    private readonly ImuSettings _settings;
    private readonly ILogger<Mpu6050> _logger;

    private I2cDevice? _mpu6050Device;
    private GpioController? _ioController;
    public event EventHandler<ImuReading>? SensorDataRetrievedEvent;

    public Mpu6050(IOptions<ImuSettings> settings, ILogger<Mpu6050> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Initialize()
    {
        try
        {
            _ioController = new GpioController(PinNumberingScheme.Logical);
            _ioController.OpenPin(_settings.InterruptPin);
            _ioController.Write(_settings.InterruptPin, PinValue.Low);
            _ioController.SetPinMode(_settings.InterruptPin, PinMode.Input);

            _mpu6050Device = I2cDevice.Create(new I2cConnectionSettings(_settings.BusId, _settings.Address));

            await Task.Delay(3);
            WriteByte(PWR_MGMT_1, 0x80);
            await Task.Delay(100);

            WriteByte(PWR_MGMT_1, 0x2);
            WriteByte(USER_CTRL, 0x04);

            WriteByte(PWR_MGMT_1, 1);
            WriteByte(GYRO_CONFIG, 0);
            WriteByte(ACCEL_CONFIG, 0);

            WriteByte(CONFIG, 1);
            WriteByte(SMPLRT_DIV, 19);
            WriteByte(FIFO_EN, 0x78);
            WriteByte(USER_CTRL, 0x40);
            WriteByte(INT_ENABLE, 0x1);

            _ioController.RegisterCallbackForPinValueChangedEvent(_settings.InterruptPin,
                PinEventTypes.Rising | PinEventTypes.Falling, Interrupt);
            _logger.LogInformation("Mpu6050 Initialized!");
        }
        catch (Exception e)
        {
            _logger.LogError("Could not initialize Mpu6050: {Exception}", e);
        }
    }

    public void AddEventHandler(EventHandler<ImuReading> handler)
    {
        SensorDataRetrievedEvent += handler;
    }

    private byte ReadByte(byte regAddr)
    {
        byte[] buffer = new byte[1];
        buffer[0] = regAddr;
        byte[] value = new byte[1];
        _mpu6050Device!.WriteRead(buffer, value);
        return value[0];
    }

    private byte[] ReadBytes(byte address, int length)
    {
        var values = new byte[length];
        var buffer = new byte[1];
        buffer[0] = address;
        _mpu6050Device!.WriteRead(buffer, values);
        return values;
    }

    private ushort ReadWord(byte address)
    {
        var buffer = ReadBytes(address, 2);
        return (ushort) ((buffer[0] << 8) | buffer[1]);
    }

    private void WriteByte(byte regAddr, byte data)
    {
        byte[] buffer = new byte[2];
        buffer[0] = regAddr;
        buffer[1] = data;
        _mpu6050Device!.Write(buffer);
    }

    private ImuReading[] GetReadings()
    {
        int interruptStatus = ReadByte(INT_STATUS);
        if ((interruptStatus & 0x10) != 0)
        {
            WriteByte(USER_CTRL, 0x44);
        }

        var readings = new List<ImuReading>();

        if ((interruptStatus & 0x1) != 0)
        {
            int count = ReadWord(FIFO_COUNT);

            while (count >= SensorBytes)
            {
                byte[] data = ReadBytes(FIFO_R_W, SensorBytes);
                count -= SensorBytes;

                var xa = (short) (data[0] << 8 | data[1]);
                var ya = (short) (data[2] << 8 | data[3]);
                var za = (short) (data[4] << 8 | data[5]);

                var xg = (short) (data[6] << 8 | data[7]);
                var yg = (short) (data[8] << 8 | data[9]);
                var zg = (short) (data[10] << 8 | data[11]);

                readings.Add(new ImuReading
                {
                    AccelerationX = (float) xa / 16384,
                    AccelerationY = (float) ya / 16384,
                    AccelerationZ = (float) za / 16384,
                    GyroscopeX = (float) xg / 131,
                    GyroscopeY = (float) yg / 131,
                    GyroscopeZ = (float) zg / 131
                });
            }
        }

        return readings.ToArray();
    }

    private void Interrupt(object sender, PinValueChangedEventArgs args)
    {
        var readings = GetReadings();

        if (!readings.Any() || SensorDataRetrievedEvent is null)
        {
            return;
        }

        foreach (var mpuSensorValue in readings)
        {
            _logger.LogInformation("Got reading from Mpu6050!");
            SensorDataRetrievedEvent(this, mpuSensorValue);
        }
    }

    public void Stop()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (_mpu6050Device is null)
        {
            return;
        }

        _mpu6050Device.Dispose();
    }
}