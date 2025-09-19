namespace YoTuViLo2.Views;

public partial class DownloadPage : ContentPage
{
    public DownloadPage()
    {
        InitializeComponent();
        BindingContext = new DownloadPageViewModel();
        Shell.SetNavBarIsVisible(this, false);
    }
}