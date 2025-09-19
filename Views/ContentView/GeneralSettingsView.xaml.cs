namespace YoTuViLo2.Views;

public partial class GeneralSettingsView : ContentView
{
    public GeneralSettingsView()
    {
        InitializeComponent();
        BindingContext = new GeneralSettingsViewViewModel();
    }
}