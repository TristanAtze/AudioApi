using NAudio.CoreAudioApi;

namespace AudioApi;

/// <summary>
/// Provides device enumeration and lookup utilities.
/// </summary>
public sealed class AudioDeviceService : IDisposable
{
    private readonly MMDeviceEnumerator _enumerator = new();

    /// <summary>
    /// Lists active playback devices.
    /// </summary>
    public IReadOnlyList<AudioDeviceInfo> ListOutputDevices()
    {
        var col = _enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        return col.Select(d => AudioDeviceInfo.FromMmDevice(d, _enumerator)).ToList();
    }

    /// <summary>
    /// Lists active recording devices.
    /// </summary>
    public IReadOnlyList<AudioDeviceInfo> ListInputDevices()
    {
        var col = _enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
        return col.Select(d => AudioDeviceInfo.FromMmDevice(d, _enumerator)).ToList();
    }

    /// <summary>
    /// Gets the default device info for the given flow and role.
    /// </summary>
    public AudioDeviceInfo GetDefault(AudioFlow flow, DeviceRole role = DeviceRole.Console)
    {
        var df = flow == AudioFlow.Render ? DataFlow.Render : DataFlow.Capture;
        var rl = role switch
        {
            DeviceRole.Console => Role.Console,
            DeviceRole.Multimedia => Role.Multimedia,
            DeviceRole.Communications => Role.Communications,
            _ => Role.Console
        };
        var dev = _enumerator.GetDefaultAudioEndpoint(df, rl);
        return AudioDeviceInfo.FromMmDevice(dev, _enumerator);
    }

    /// <summary>
    /// Retrieves device info by Windows device ID.
    /// </summary>
    public AudioDeviceInfo? GetById(string deviceId)
    {
        try
        {
            var dev = _enumerator.GetDevice(deviceId);
            return AudioDeviceInfo.FromMmDevice(dev, _enumerator);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Resolves an MMDevice for advanced operations.
    /// </summary>
    public MMDevice? ResolveMmDevice(string deviceId)
    {
        try { return _enumerator.GetDevice(deviceId); } catch { return null; }
    }

    /// <summary>
    /// Releases unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _enumerator.Dispose();
    }
}
