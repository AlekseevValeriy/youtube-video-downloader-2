namespace YoTuViLo2.ViewModels;

internal partial class SettingsPageViewModel : INotifyPropertyChanged
{
    List<ContentView> _pages = new List<ContentView>()
    {
        new GeneralSettingsView(),
        new ToolsSettingsView()
    };

    public List<ContentView> Pages
    {
        get => _pages;
        set
        {
            if (value != _pages)
            {
                _pages = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
