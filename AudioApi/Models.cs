using NAudio.CoreAudioApi;
using System.Data;


namespace AudioApi;


/// <summary>
/// Immutable device snapshot used by the API.
/// </summary>
public sealed record AudioDeviceInfo(
string Id,
string Name,
AudioFlow Flow,
bool IsDefaultConsole,
bool IsDefaultMultimedia,
bool IsDefaultCommunications,
bool IsMuted,
float MasterVolume
)
{
    /// <summary>
    /// Creates a device snapshot from an MMDevice.
    /// </summary>
    public static AudioDeviceInfo FromMmDevice(MMDevice mm, MMDeviceEnumerator enu)
    {
        var flow = mm.DataFlow == DataFlow.Render ? AudioFlow.Render : AudioFlow.Capture;
        bool defConsole = mm.ID == enu.GetDefaultAudioEndpoint(mm.DataFlow, Role.Console).ID;
        bool defMulti = mm.ID == enu.GetDefaultAudioEndpoint(mm.DataFlow, Role.Multimedia).ID;
        bool defComm = mm.ID == enu.GetDefaultAudioEndpoint(mm.DataFlow, Role.Communications).ID;
        float vol = 0f;
        bool muted = false;
        try
        {
            vol = mm.AudioEndpointVolume?.MasterVolumeLevelScalar ?? 0f;
            muted = mm.AudioEndpointVolume?.Mute ?? false;
        }
        catch { }
        return new AudioDeviceInfo(mm.ID, mm.FriendlyName, flow, defConsole, defMulti, defComm, muted, vol);
    }
}