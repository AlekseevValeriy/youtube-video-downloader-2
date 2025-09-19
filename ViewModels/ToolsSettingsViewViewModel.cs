namespace YoTuViLo2.ViewModels;
internal partial class ToolsSettingsViewViewModel : INotifyPropertyChanged
{
    String _downloadTool = AppSettings.DownloadToolPath;
    String _convertTool = AppSettings.ConvertToolPath;

    public String DownloadTool
    {
        get => _downloadTool;
        set
        {
            if (value != _downloadTool)
            {
                _downloadTool = value;
                OnPropertyChanged();
            }
        }
    }
    public String ConvertTool
    {
        get => _convertTool;
        set
        {
            if (value != _convertTool)
            {
                _convertTool = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SetDownloadToolPathCommand { get; set; }
    public ICommand SetConvertToolPathCommand { get; set; }
    public ICommand SaveCommand { get; set; }

    public ToolsSettingsViewViewModel()
    {
        SetDownloadToolPathCommand = new Command(SetDownloadToolPath);
        SetConvertToolPathCommand = new Command(SetConvertToolPath);
        SaveCommand = new Command(Save);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    async void SetDownloadToolPath()
    {
        FileResult? result = await FilePicker.PickAsync();

        if (result is not null)
        {
            DownloadTool = result.FullPath;
        }
    }

    async void SetConvertToolPath()
    {
        FileResult? result = await FilePicker.PickAsync();

        if (result is not null)
        {
            ConvertTool = result.FullPath;
        }
    }

    void Save()
    {
        AppSettings.DownloadToolPath = DownloadTool;
        AppSettings.ConvertToolPath = ConvertTool;
    }
}
