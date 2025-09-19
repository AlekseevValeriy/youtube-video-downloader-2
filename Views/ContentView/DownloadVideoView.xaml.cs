using YoTuViLo2.Services;

namespace YoTuViLo2.Views;

public partial class DownloadVideoView : ContentView
{
    public DownloadVideoView()
    {
        InitializeComponent();
    }

    private void StartDownloadClicked(Object sender, EventArgs e)
    {
        Process downloadTask = new VideoLoaderBuilder()
            .FromSource("https://www.youtube.com/watch?v=VMmMaOFSKFU&pp=0gcJCa0JAYcqIYzv")
            .BuildToProcess();

        downloadTask.OutputDataReceived += OutputDataReceived;
        downloadTask.ErrorDataReceived += OutputDataReceived;

        downloadTask.Start();

        downloadTask.BeginErrorReadLine();
        downloadTask.BeginOutputReadLine();
    }

    void OutputDataReceived(Object? sender, DataReceivedEventArgs e)
    {
        if (e.Data is null) return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Output.Text += e?.Data + "\n";
        });

    }
}