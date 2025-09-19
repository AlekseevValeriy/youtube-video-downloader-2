namespace YoTuViLo2.Helpers;

internal static class AppPaths
{
    internal static readonly String AppDirectoryPath = "..\\..\\..\\..";

    internal static readonly String DefaultYtDlpPath = Path.Combine(AppDirectoryPath, "Tools", "yt-dlp.exe");
    internal static readonly String DefaultFFmpegPath = Path.Combine(AppDirectoryPath, "Tools", "FFmpeg", "bin", "ffmpeg.exe");

    internal static readonly String DefaultDownloadPath = Path.Combine("C:", "UploadedVideos");

    internal static readonly String ConfigFilePath = Path.Combine(AppDirectoryPath, "Data", "Config.json");
}
