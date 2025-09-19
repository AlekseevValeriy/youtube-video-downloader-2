using Windows.Storage.Pickers;
using Windows.Storage;

using YoTuViLo2.Services;

namespace YoTuViLo2.ViewModels;

internal partial class DownloadPageViewModel : INotifyPropertyChanged
{
    readonly List<DownloadScope> _downloadScopes = new(Enum.GetValues(typeof(DownloadScope)).Cast<DownloadScope>());
    readonly List<ContentView> _pages = new List<ContentView>()
    {
        new DownloadVideoView(),
        new DownloadPlaylistView(),
        new DownloadAuthorView(),
    };
    readonly YTDLPOutputParser _outputParser = new();

    Process _downloadTask = new Process();
    String _source = "";
    DownloadScope _scope = DownloadScope.Best;
    String _geoBypass = "";
    Double _allBarProgress = 0.0;
    Double _eachBarProgress = 0.0;
    String _allBarProgressText = "All bar";
    String _eachBarProgressText = "Each bar";
    String _debugOutput = "";
    Boolean _audioIsChecked = false;
    Boolean _videoIsChecked = false;
    String _currentVideoDownloadTarget = "None";
    String _currentPlaylistDownloadTarget = "None";
    Boolean _isDownloadDataWriteable = true;
    String _generalStatus = "None";
    Boolean _isAudioExtract = false;
    Boolean _withMetadata= false;
    Boolean _isOnlyMusic = false;
    String _downloadPath = "";
    Int32 _debugCursorPosition = 0;

    public Int32 DebugCursorPosition
    {
        get => _debugCursorPosition;
        set
        {
            if (value != _debugCursorPosition)
            {
                _debugCursorPosition = value;
                OnPropertyChanged();
            }
        }
    }
    public String DownloadPath
    {
        get => _downloadPath;
        set
        {
            if (value != _downloadPath)
            {
                _downloadPath = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean IsAudioExtract
    {
        get => _isAudioExtract;
        set
        {
            if (value != _isAudioExtract)
            {
                _isAudioExtract = value;
                OnPropertyChanged();
            }    
        }
    }
    public Boolean WithMetadata
    {
        get => _withMetadata;
        set
        {
            if (value != _withMetadata)
            {
                _withMetadata = value;
                OnPropertyChanged();
            }    
        }
    }
    public String GeneralStatus
    {
        get => _generalStatus;
        set
        {
            if (value != _generalStatus)
            {
                _generalStatus = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean IsDownloadDataWriteable
    {
        get => _isDownloadDataWriteable;
        set
        {
            if (value != _isDownloadDataWriteable)
            {
                _isDownloadDataWriteable = value;
                OnPropertyChanged();
            }
        }
    }
    public String CurrentPlaylistDownloadTarget
    {
        get => _currentPlaylistDownloadTarget;
        set
        {
            if (value != _currentPlaylistDownloadTarget)
            {
                _currentPlaylistDownloadTarget = value;
                OnPropertyChanged();
            }
        }
    }
    public String CurrentVideoDownloadTarget
    {
        get => _currentVideoDownloadTarget;
        set
        {
            if (value != _currentVideoDownloadTarget)
            {
                _currentVideoDownloadTarget = value;
                OnPropertyChanged();
            }
        }
    }
    public DownloadFormat? GetFormat
    {
        get
        {
            if (AudioIsChecked & VideoIsChecked) return DownloadFormat.Audio & DownloadFormat.Video;
            else if (AudioIsChecked) return DownloadFormat.Audio;
            else if (VideoIsChecked) return DownloadFormat.Video;
            else return null;
        }
    }
    public Boolean AudioIsChecked
    {
        get => _audioIsChecked;
        set
        {
            if (value != _audioIsChecked)
            {
                _audioIsChecked = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean IsOnlyMusic {
        get => _isOnlyMusic;
        set
        {
            if (value != _isOnlyMusic)
            {
                _isOnlyMusic = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean VideoIsChecked
    {
        get => _videoIsChecked;
        set
        {
            if (value != _videoIsChecked)
            {
                _videoIsChecked = value;
                OnPropertyChanged();
            }
        }
    }
    public String DebugOutput
    {
        get => _debugOutput;
        set
        {
            if (value != _debugOutput)
            {
                _debugOutput = value;
                OnPropertyChanged();
            }
        }

    }
    public Double AllBarProgress
    {
        get => _allBarProgress;
        set
        {
            if (value != _allBarProgress)
            {
                _allBarProgress = value;
                OnPropertyChanged();
            }
        }
    }
    public Double EachBarProgress
    {
        get => _eachBarProgress;
        set
        {
            if (value != _eachBarProgress)
            {
                _eachBarProgress = value;
                OnPropertyChanged();
            }
        }
    }
    public String AllBarProgressText
    {
        get => _allBarProgressText;
        set
        {
            if (value != _allBarProgressText)
            {
                _allBarProgressText = value;
                OnPropertyChanged();
            }
        }
    }
    public String EachBarProgressText
    {
        get => _eachBarProgressText;
        set
        {
            if (value != _eachBarProgressText)
            {
                _eachBarProgressText = value;
                OnPropertyChanged();
            }
        }
    }
    public String GeoBypass
    {
        get => _geoBypass;
        set
        {
            if (value != _geoBypass)
            {
                _geoBypass = value;
                OnPropertyChanged();
            }
        }
    }
    public List<DownloadScope> DownloadScopes => _downloadScopes;
    public String Source
    {
        get => _source;
        set
        {
            if (value != _source)
            {
                _source = value;
                OnPropertyChanged();
            }
        }
    }
    public DownloadScope Scope
    {
        get => _scope;
        set
        {
            if (value != _scope)
            {
                _scope = value;
                OnPropertyChanged();
            }
        }
    }

    public List<ContentView> Pages => _pages;
    public ICommand StartDownloadCommand { get; set; }
    public ICommand SetDownloadPathCommand { get; set; }

    public DownloadPageViewModel()
    {
        StartDownloadCommand = new Command(DownloadLaunch);
        SetDownloadPathCommand = new Command(SetOutputLocationPath);

        _outputParser.SetStatusStartTuneUp += SetTuneUpStatus;
        _outputParser.SetStatusStartVideoDownload += SetVideoDownloadStatus;
        _outputParser.SetStatusEndVideoDownload += SetVideoDownloadedStatus;
        _outputParser.SetStatusEndPlaylistDownload += SetPlaylistDownloadedStatus;
        _outputParser.SetVideoTitle += SetVideoTarget;
        _outputParser.SetPlaylistTitle += SetPlaylistTarget;
        _outputParser.SetEachProgress += SetEachProgress;
        _outputParser.SetAllProgress += SetAllProgress;
        _outputParser.SetEachProgressAsText += SetEachBarText;
        _outputParser.SetAllProgressAsText += SetAllBarText;

        DebugCursorPosition = DebugOutput.Length;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    void ProcessInit()
    {
        _downloadTask.OutputDataReceived += ProcessOutputRedirect;
        _downloadTask.ErrorDataReceived += ProcessOutputRedirect;

        _downloadTask.EnableRaisingEvents = true;
        _downloadTask.Exited += EndDownload;

        _downloadTask.OutputDataReceived += _outputParser.Parse;
    }

    void DownloadLaunch()
    {
        VideoLoaderBuilder builder = VideoLoaderBuilder.New()
            .FromSource(Source.Split("\n").ToList())
            .WithScope(Scope)
            .AsFormat(GetFormat)
            .InDirectory(DownloadPath)
            .WithAudioExtract(IsAudioExtract)
            .OnlyMusic(IsOnlyMusic)
            .WithMetadata(WithMetadata)
            .BuildEnd();

        _downloadTask = builder
            .BuildToProcess();

        ProcessInit();

        _downloadTask.Start();

# if DEBUG
        DebugOutput += $"{builder.GetCommand()}\n";
        DebugCursorPosition = DebugOutput.Length;
# endif

        StartDownload();
    }

    void StartDownload()
    {
        IsDownloadDataWriteable = false;

        _downloadTask.BeginErrorReadLine();
        _downloadTask.BeginOutputReadLine();

        CurrentVideoDownloadTarget = "...";
        CurrentPlaylistDownloadTarget = "...";
        GeneralStatus = "...";
    }

    void EndDownload(Object? sender, EventArgs e)
    {
        CurrentVideoDownloadTarget = "None";
        CurrentPlaylistDownloadTarget = "None";
        IsDownloadDataWriteable = true;
        GeneralStatus = "None";
        AllBarProgress = 0;
        EachBarProgress = 0;
        EachBarProgressText = "Each bar";
        AllBarProgressText = "All bar";
    }

    void ProcessOutputRedirect(Object sender, DataReceivedEventArgs e)
    {
        DebugOutput += $"{e.Data}\n";
        DebugCursorPosition = DebugOutput.Length;
    }

    void SetVideoTarget(String text)
    {
        CurrentVideoDownloadTarget = text;
        EachBarProgress = 0;
    }

    void SetPlaylistTarget(String text)
    {
        CurrentPlaylistDownloadTarget = text;
    }

    void SetTuneUpStatus()
    {
        GeneralStatus = "Tune-up";
    }

    void SetVideoDownloadStatus()
    {
        GeneralStatus = "Video downloading";
    }

    void SetVideoDownloadedStatus()
    {
        GeneralStatus = "Video downloaded!";
    }

    void SetPlaylistDownloadedStatus()
    {
        AllBarProgress = 1;
        GeneralStatus = "Playlist downloaded!";
    }

    void SetEachProgress(Double percent)
    {
        EachBarProgress = percent / 100.0;
    }

    void SetAllProgress(Double percent)
    {
        AllBarProgress = percent;
    }

    async void SetOutputLocationPath()
    {
        FolderPicker folderPicker = new FolderPicker();
        folderPicker.FileTypeFilter.Add("*");
        nint hwnd = (App.Current?.Windows[0].Handler.PlatformView as MauiWinUIWindow)!.WindowHandle;
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

        StorageFolder? result = await folderPicker.PickSingleFolderAsync();

        if (result is not null)
        {
            DownloadPath = result.Path;
        }
    }

    void SetEachBarText(String text)
    {
        EachBarProgressText = $"Each bar [{text}]";
    }

    void SetAllBarText(String text)
    {
        AllBarProgressText = $"All bar [{text}]";
    }
}