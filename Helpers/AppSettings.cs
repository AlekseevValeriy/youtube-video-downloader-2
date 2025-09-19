using Cherub.Helpers;

namespace YoTuViLo2.Helpers;

internal static class AppSettings
{
    static String _downloadToolPath = AppPaths.DefaultYtDlpPath;
    static String _convertToolPath = AppPaths.DefaultFFmpegPath;
    static String _downloadPath = AppPaths.DefaultDownloadPath;

    internal static String DownloadToolPath {
        get => _downloadToolPath;
        set
        {
            _downloadToolPath = value; 
            Config.SaveParameter(nameof(DownloadToolPath), value);
        } 
    }
    internal static String ConvertToolPath
    {
        get => _convertToolPath; 
        set 
        {
            _convertToolPath = value; 
            Config.SaveParameter(nameof(ConvertToolPath), value); 
        } 
    }
    internal static String DownloadPath 
    {
        get => _downloadPath;
        set 
        { 
            _downloadPath = value; 
            Config.SaveParameter(nameof(DownloadPath), value); 
        } 
    }

    static AppSettings()
    {
        Config.LoadConfig();
    }
}
