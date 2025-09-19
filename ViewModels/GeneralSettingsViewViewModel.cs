using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace YoTuViLo2.ViewModels;
internal partial class GeneralSettingsViewViewModel : INotifyPropertyChanged
{
    String _outputLocation = AppSettings.DownloadPath;

    public String OutputLocation
    {
        get => _outputLocation;
        set
        {
            if (value != _outputLocation)
            {
                _outputLocation = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SetOutputLocationPathCommand { get; set; }
    public ICommand SaveCommand { get; set; }

    public GeneralSettingsViewViewModel()
    {
        SetOutputLocationPathCommand = new Command(SetOutputLocationPath);
        SaveCommand = new Command(Save);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    async public void SetOutputLocationPath()
    {
        FolderPicker folderPicker = new FolderPicker();
        folderPicker.FileTypeFilter.Add("*");
        nint hwnd = (App.Current?.Windows[0].Handler.PlatformView as MauiWinUIWindow)!.WindowHandle;
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

        StorageFolder? result = await folderPicker.PickSingleFolderAsync();

        if (result is not null)
        {
            OutputLocation = result.Path;
        }
    }

    void Save()
    {
        AppSettings.DownloadPath = OutputLocation;
    }
}
