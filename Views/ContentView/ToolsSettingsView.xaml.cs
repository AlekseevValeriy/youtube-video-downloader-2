namespace YoTuViLo2.Views;

public partial class ToolsSettingsView : ContentView
{
    public ToolsSettingsView()
    {
        InitializeComponent();
        BindingContext = new ToolsSettingsViewViewModel();
    }
}