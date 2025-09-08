using NAudio.CoreAudioApi;


namespace AudioApi;


/// <summary>
/// Provides peak meter readings for devices.
/// </summary>
public sealed class AudioMeter
{
    private readonly AudioDeviceService _devices;


    /// <summary>
    /// Creates a new instance using the provided device service.
    /// </summary>
    public AudioMeter(AudioDeviceService devices)
    {
        _devices = devices;
    }


    /// <summary>
    /// Gets instantaneous peak level (0..1) for a device.
    /// </summary>
    public float GetPeak(string deviceId)
    {
        var dev = _devices.ResolveMmDevice(deviceId) ?? throw new InvalidOperationException("Device not found");
        return dev.AudioMeterInformation?.MasterPeakValue ?? 0f;
    }
}