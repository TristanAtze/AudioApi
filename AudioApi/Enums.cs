namespace AudioApi;


/// <summary>
/// Audio signal flow direction.
/// </summary>
public enum AudioFlow
{
    /// <summary>
    /// Playback devices (speakers, headphones).
    /// </summary>
    Render,
    /// <summary>
    /// Recording devices (microphones, line-in).
    /// </summary>
    Capture
}


/// <summary>
/// Common Windows device roles.
/// </summary>
public enum DeviceRole
{
    /// <summary>
    /// Default for most apps.
    /// </summary>
    Console,
    /// <summary>
    /// Default for music and video.
    /// </summary>
    Multimedia,
    /// <summary>
    /// Default for VoIP and calls.
    /// </summary>
    Communications
}