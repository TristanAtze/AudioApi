using NAudio.Wave;


namespace AudioApi.Playback;


/// <summary>
/// High-level audio player supporting file and stream playback.
/// </summary>
public sealed class AudioPlayer : IDisposable
{
    private IWavePlayer? _output;
    private WaveStream? _reader;
    private readonly object _gate = new();


    /// <summary>
    /// True while a file is loaded.
    /// </summary>
    public bool IsLoaded => _reader is not null;


    /// <summary>
    /// Current playback state.
    /// </summary>
    public PlaybackState State => _output?.PlaybackState ?? PlaybackState.Stopped;


    /// <summary>
    /// Total duration if loaded.
    /// </summary>
    public TimeSpan? Duration => _reader?.TotalTime;
    /// <summary>
    /// Current position if loaded.
    /// </summary>
    public TimeSpan? Position
    {
        get => _reader?.CurrentTime;
        set { if (_reader is not null && value is not null) _reader.CurrentTime = value.Value; }
    }


    /// <summary>
    /// Loads and prepares a file for playback on the default output device.
    /// </summary>
    public void Load(string path)
    {
        lock (_gate)
        {
            UnloadInternal();
            _reader = CreateReader(path);
            _output = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 200);
            _output.Init(_reader);
        }
    }
    /// <summary>
    /// Starts or resumes playback.
    /// </summary>
    public void Play()
    {
        lock (_gate)
        {
            if (_output is null) throw new InvalidOperationException("Nothing loaded");
            _output.Play();
        }
    }


    /// <summary>
    /// Pauses playback.
    /// </summary>
    public void Pause()
    {
        lock (_gate)
        {
            _output?.Pause();
        }
    }

    /// <summary>
    /// Stops playback and resets position to start.
    /// </summary>
    public void Stop()
    {
        lock (_gate)
        {
            if (_output is null) return;
            _output.Stop();
            if (_reader is not null) _reader.Position = 0;
        }
    }


    /// <summary>
    /// Unloads the current file and frees resources.
    /// </summary>
    public void Unload()
    {
        lock (_gate)
        {
            UnloadInternal();
        }
    }

    private void UnloadInternal()
    {
        _output?.Dispose();
        _output = null;
        _reader?.Dispose();
        _reader = null;
    }


    private static WaveStream CreateReader(string path)
    {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return ext switch
        {
            ".wav" => new WaveFileReader(path),
            ".mp3" => new Mp3FileReader(path),
            ".aiff" or ".aif" => new AiffFileReader(path),
            _ => new AudioFileReader(path)
        };
    }

    /// <summary>
    /// Disposes the player.
    /// </summary>
    public void Dispose()
    {
        Unload();
    }
}