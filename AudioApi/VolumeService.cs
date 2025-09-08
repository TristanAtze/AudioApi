using NAudio.CoreAudioApi;


namespace AudioApi;


/// <summary>
/// Controls per-endpoint master volume and mute state.
/// </summary>
public sealed class VolumeService
{
    private readonly AudioDeviceService _devices;


    /// <summary>
    /// Creates a new instance using the provided device service.
    /// </summary>
    public VolumeService(AudioDeviceService devices)
    {
        _devices = devices;
    }


    /// <summary>
    /// Reads the master volume (0..1) for a device.
    /// </summary>
    public float GetMasterVolume(string deviceId)
    {
        var dev = _devices.ResolveMmDevice(deviceId) ?? throw new InvalidOperationException("Device not found");
        return dev.AudioEndpointVolume.MasterVolumeLevelScalar;
    }


    /// <summary>
    /// Sets the master volume (0..1) for a device.
    /// </summary>
    public void SetMasterVolume(string deviceId, float volume01)
    {
        var vol = Math.Clamp(volume01, 0f, 1f);
        var dev = _devices.ResolveMmDevice(deviceId) ?? throw new InvalidOperationException("Device not found");
        dev.AudioEndpointVolume.MasterVolumeLevelScalar = vol;
    }


    /// <summary>
    /// Gets the mute state of a device.
    /// </summary>
    public bool GetMute(string deviceId)
    {
        var dev = _devices.ResolveMmDevice(deviceId) ?? throw new InvalidOperationException("Device not found");
        return dev.AudioEndpointVolume.Mute;
    }


    /// <summary>
    /// Sets the mute state of a device.
    /// </summary>
    public void SetMute(string deviceId, bool mute)
    {
        var dev = _devices.ResolveMmDevice(deviceId) ?? throw new InvalidOperationException("Device not found");
        dev.AudioEndpointVolume.Mute = mute;
    }
}