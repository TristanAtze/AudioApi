using NAudio.CoreAudioApi;
using NAudio.Wave;
public sealed class MicrophoneCapture : IDisposable
{
    private WasapiCapture? _capture;
    private WaveFileWriter? _writer;
    private readonly object _gate = new();


    /// <summary>
    /// True while capturing.
    /// </summary>
    public bool IsCapturing { get; private set; }


    /// <summary>
    /// Starts recording to a WAV file.
    /// </summary>
    public void Start(string deviceId, string wavPath)
    {
        lock (_gate)
        {
            if (IsCapturing) throw new InvalidOperationException("Already capturing");
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(wavPath))!);
            _capture = new WasapiCapture(new NAudio.CoreAudioApi.MMDeviceEnumerator().GetDevice(deviceId));
            _writer = new WaveFileWriter(wavPath, _capture.WaveFormat);
            _capture.DataAvailable += (s, a) => _writer?.Write(a.Buffer, 0, a.BytesRecorded);
            _capture.RecordingStopped += (s, a) => StopInternal();
            _capture.StartRecording();
            IsCapturing = true;
        }
    }


    /// <summary>
    /// Stops recording and flushes the file.
    /// </summary>
    public void Stop()
    {
        lock (_gate)
        {
            if (!IsCapturing) return;
            _capture?.StopRecording();
        }
    }


    private void StopInternal()
    {
        lock (_gate)
        {
            _capture?.Dispose();
            _capture = null;
            _writer?.Dispose();
            _writer = null;
            IsCapturing = false;
        }
    }


    /// <summary>
    /// Disposes resources.
    /// </summary>
    public void Dispose()
    {
        Stop();
        _capture?.Dispose();
        _writer?.Dispose();
    }
}