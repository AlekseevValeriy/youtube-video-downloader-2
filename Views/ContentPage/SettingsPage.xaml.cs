namespace YoTuViLo2.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = new SettingsPageViewModel();
        Shell.SetNavBarIsVisible(this, false);
    }
}