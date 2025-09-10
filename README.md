# AudioApi 🎵

A lightweight .NET library to simplify working with audio devices, playback, and recording.  
It wraps common audio operations into an easy-to-use API without requiring deep knowledge of low-level Windows APIs.

## ✨ Features
- List available input and output audio devices  
- Play audio files (e.g., WAV, MP3)  
- Record audio from microphone  
- Control volume and mute state  
- Switch between default devices  

## 📦 Installation
Install via [NuGet](https://www.nuget.org/):

```powershell
dotnet add package AudioApi
````

Or via the Package Manager Console:

```powershell
Install-Package AudioApi
```

## 🚀 Usage

### List available devices

```csharp
using AudioApi;

var devices = AudioManager.GetOutputDevices();
foreach (var device in devices)
{
    Console.WriteLine($"{device.Id}: {device.Name}");
}
Console.ReadKey();
```

### Play an audio file

```csharp
using AudioApi;

var player = new AudioPlayer();
player.Play("test.mp3");

Console.WriteLine("Playing... press any key to stop");
Console.ReadKey();

player.Stop();
```

### Record from microphone

```csharp
using AudioApi;

var recorder = new AudioRecorder();
recorder.Start("recorded.wav");

Console.WriteLine("Recording... press any key to stop");
Console.ReadKey();

recorder.Stop();
```

## ⚙️ Requirements

* .NET 6.0 or later
* Windows 10/11 (uses Windows Audio APIs under the hood)

## 📖 Roadmap

* [ ] Streaming audio from network sources
* [ ] Real-time audio processing (FFT, filters, etc.)
* [ ] Cross-platform support (Linux/Mac via ALSA/CoreAudio)

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!
Feel free to open a PR or issue on [GitHub](https://github.com/TristanAtze/AudioApi).

## 📜 License

MIT License © 2025 Ben
