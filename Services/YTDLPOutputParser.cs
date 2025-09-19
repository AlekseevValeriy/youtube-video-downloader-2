using System.Text.RegularExpressions;

namespace YoTuViLo2.Services;

internal partial class YTDLPOutputParser
{
    public event Action? SetStatusStartTuneUp;
    public event Action? SetStatusStartVideoDownload;
    public event Action? SetStatusEndVideoDownload;
    public event Action? SetStatusEndPlaylistDownload;

    public delegate void MessageHandler(String text);
    public delegate void ProgressHandler(Double text);

    public event MessageHandler? SetVideoTitle;
    public event MessageHandler? SetPlaylistTitle;
    public event ProgressHandler? SetEachProgress;
    public event ProgressHandler? SetAllProgress;
    public event MessageHandler? SetEachProgressAsText;
    public event MessageHandler? SetAllProgressAsText;
    public event MessageHandler? SetError;

    public void Parse(Object sender, DataReceivedEventArgs e)
    {
        if (e.Data is null ) return;

        String message = e.Data;

        if (StartTuneUpRegex().IsMatch(message))
        {
            SetStatusStartTuneUp?.Invoke();
        }
        else if (StartVideoDownloadRegex().IsMatch(message))
        {
            SetStatusStartVideoDownload?.Invoke();
        }
        else if (EndVideoDownloadRegex().IsMatch(message))
        {
            SetStatusEndVideoDownload?.Invoke();
        }
        else if (SetVideoLinkRegex().IsMatch(message))
        {
            SetVideoTitle?.Invoke(message.Replace("[youtube] Extracting URL: ", ""));
        }
        else if (SetVideoTitleRegex().IsMatch(message))
        {
            SetVideoTitle?.Invoke(message.Replace("[download] Destination: ", ""));
        }
        else if (SetPlaylistLinkRegex().IsMatch(message))
        {
            SetPlaylistTitle?.Invoke(message.Replace("[youtube:tab] Extracting URL: ", ""));
        }
        else if (SetPlaylistTitleRegex().IsMatch(message))
        {
            SetPlaylistTitle?.Invoke(message.Replace("[download] Downloading playlist: ", ""));
        }
        else if (SetEachDownloadProgressRegex().IsMatch(message))
        {
            if (DownloadProgressRegex().IsMatch(message))
            {
                String spercent = DownloadProgressRegex().Match(message).Value;
                SetEachProgressAsText?.Invoke(spercent);

                if (Double.TryParse(spercent.Replace("%", "").Replace(".", ","), out Double percent))
                {
                    SetEachProgress?.Invoke(percent);
                    
                }
            }
        }
        else if (SetAllDownloadProgressRegex().IsMatch(message))
        {
            if (AllDownloadProgressRegex().IsMatch(message))
            {
                String count = AllDownloadProgressRegex().Match(message).Value.Replace("item ", "");

                SetAllProgressAsText?.Invoke(count);

                String[] sCount = count.Split(" of ");

                if (sCount.Length == 2)
                {
                    Double f = Double.Parse(sCount[0]);
                    Double s = Double.Parse(sCount[1]);

                    SetAllProgress?.Invoke((f - 1) / s);
                }
            }
        }
        else if (EndPlaylistDownloadRegex().IsMatch(message))
        {
            SetStatusEndPlaylistDownload?.Invoke();
        }
    }

    [GeneratedRegex(@".*cookies from.*")]
    private static partial Regex StartTuneUpRegex();

    [GeneratedRegex(@"\[download\] Sleeping.*")]
    private static partial Regex StartVideoDownloadRegex();

    [GeneratedRegex(@"\[download\].*of.*in.*at.*")]
    private static partial Regex EndVideoDownloadRegex();

    [GeneratedRegex(@"\[download\] Finished downloading playlist:.*")]
    private static partial Regex EndPlaylistDownloadRegex();

    [GeneratedRegex(@"\[youtube\] Extracting URL:.*")]
    private static partial Regex SetVideoLinkRegex();

    [GeneratedRegex(@"\[download\] Destination:.*")]
    private static partial Regex SetVideoTitleRegex();

    [GeneratedRegex(@"\[youtube:tab\] Extracting URL:.*playlist.*")]
    private static partial Regex SetPlaylistLinkRegex();

    [GeneratedRegex(@"\[download\] Downloading playlist:.*")]
    private static partial Regex SetPlaylistTitleRegex();

    [GeneratedRegex(@"\d*\.\d*%")]
    private static partial Regex DownloadProgressRegex();

    [GeneratedRegex(@"\[download\].*of.*at.*ETA.*")]
    private static partial Regex SetEachDownloadProgressRegex();

    [GeneratedRegex(@"\[download\] Downloading item \d*.*\d*")]
    private static partial Regex SetAllDownloadProgressRegex();

    [GeneratedRegex(@"item \d* of \d*")]
    private static partial Regex AllDownloadProgressRegex();
}

/* video debug
System.Diagnostics.ProcessStartInfoExtracting 
cookies from firefox                                                                                        -- Start tune-up
Extracted 65 cookies from firefox
[youtube] Extracting URL: https://www.youtube.com/watch?v=vwXTBQFq_3E                                       -- Set vidoe downloaded link
[youtube] vwXTBQFq_3E: Downloading webpage
[youtube] vwXTBQFq_3E: Downloading tv client config
[youtube] vwXTBQFq_3E: Downloading player 79e70f61-main
[youtube] vwXTBQFq_3E: Downloading tv player API JSON
[info] vwXTBQFq_3E: Downloading 1 format(s): 18
[download] Sleeping 6.00 seconds as required by the site...                                                 -- Start downloading
[download] Destination: C:\UploadedVideos\oskar med k - Make Me Feel (Official Video) [vwXTBQFq_3E].mp4     -- Set vidoe downloaded title

[download]   0.0% of   13.20MiB at    1.59KiB/s ETA 02:21:44                                                -- Set vidoe downloaded process and \/ \/ \/
[download]   0.0% of   13.20MiB at    4.12KiB/s ETA 54:40   
[download]   0.0% of   13.20MiB at    9.18KiB/s ETA 24:31
[download]   0.1% of   13.20MiB at   19.24KiB/s ETA 11:42
[download]   0.2% of   13.20MiB at   24.08KiB/s ETA 09:20
[download]   0.4% of   13.20MiB at   35.82KiB/s ETA 06:16
[download]   0.7% of   13.20MiB at   44.90KiB/s ETA 04:58
[download]   1.2% of   13.20MiB at   47.98KiB/s ETA 04:38
[download]   1.6% of   13.20MiB at   51.30KiB/s ETA 04:19
[download]   2.1% of   13.20MiB at   58.18KiB/s ETA 03:47
[download]   2.9% of   13.20MiB at   62.19KiB/s ETA 03:31
[download]   3.4% of   13.20MiB at   68.44KiB/s ETA 03:10
[download]   4.4% of   13.20MiB at   75.18KiB/s ETA 02:51
[download]   5.3% of   13.20MiB at   81.36KiB/s ETA 02:37
[download]   6.3% of   13.20MiB at   87.29KiB/s ETA 02:25
[download]   7.4% of   13.20MiB at   93.66KiB/s ETA 02:13
[download]   8.6% of   13.20MiB at  101.13KiB/s ETA 02:02
[download]  10.0% of   13.20MiB at  109.27KiB/s ETA 01:51
[download]  11.6% of   13.20MiB at  119.18KiB/s ETA 01:40
[download]  13.6% of   13.20MiB at  131.33KiB/s ETA 01:28
[download]  16.0% of   13.20MiB at  146.12KiB/s ETA 01:17
[download]  19.0% of   13.20MiB at  163.97KiB/s ETA 01:06
[download]  22.6% of   13.20MiB at  181.23KiB/s ETA 00:57
[download]  25.6% of   13.20MiB at  190.66KiB/s ETA 00:52
[download]  27.9% of   13.20MiB at  198.26KiB/s ETA 00:49
[download]  30.6% of   13.20MiB at  205.86KiB/s ETA 00:45
[download]  33.1% of   13.20MiB at  214.40KiB/s ETA 00:42
[download]  36.3% of   13.20MiB at  221.62KiB/s ETA 00:38
[download]  38.8% of   13.20MiB at  228.73KiB/s ETA 00:36
[download]  41.9% of   13.20MiB at  236.54KiB/s ETA 00:33
[download]  45.0% of   13.20MiB at  243.32KiB/s ETA 00:30
[download]  47.9% of   13.20MiB at  247.24KiB/s ETA 00:28
[download]  50.4% of   13.20MiB at  253.76KiB/s ETA 00:26
[download]  54.3% of   13.20MiB at  259.88KiB/s ETA 00:23
[download]  57.1% of   13.20MiB at  263.48KiB/s ETA 00:22
[download]  59.7% of   13.20MiB at  266.59KiB/s ETA 00:20
[download]  62.4% of   13.20MiB at  269.16KiB/s ETA 00:18
[download]  64.9% of   13.20MiB at  273.00KiB/s ETA 00:17
[download]  68.0% of   13.20MiB at  277.24KiB/s ETA 00:15
[download]  71.1% of   13.20MiB at  282.65KiB/s ETA 00:13
[download]  72.2% of   13.20MiB at  281.84KiB/s ETA 00:13
[download]  72.2% of   13.20MiB at    4.46KiB/s ETA 14:03
[download]  72.2% of   13.20MiB at   13.27KiB/s ETA 04:43
[download]  72.2% of   13.20MiB at   30.95KiB/s ETA 02:01
[download]  72.3% of   13.20MiB at   66.33KiB/s ETA 00:56
[download]  72.4% of   13.20MiB at  137.08KiB/s ETA 00:27
[download]  72.6% of   13.20MiB at  148.30KiB/s ETA 00:24
[download]  73.1% of   13.20MiB at  200.44KiB/s ETA 00:18
[download]  74.0% of   13.20MiB at  304.39KiB/s ETA 00:11
[download]  75.9% of   13.20MiB at  351.53KiB/s ETA 00:09
[download]  79.0% of   13.20MiB at  452.00KiB/s ETA 00:06
[download]  84.2% of   13.20MiB at  437.77KiB/s ETA 00:04
[download]  87.3% of   13.20MiB at  451.65KiB/s ETA 00:03
[download]  91.1% of   13.20MiB at  423.15KiB/s ETA 00:02
[download]  93.6% of   13.20MiB at  423.58KiB/s ETA 00:02
[download]  96.7% of   13.20MiB at  420.39KiB/s ETA 00:01
[download]  99.7% of   13.20MiB at  407.88KiB/s ETA 00:00
[download] 100.0% of   13.20MiB at  412.15KiB/s ETA 00:00
[download] 100% of   13.20MiB in 00:00:45 at 295.28KiB/s                                                        -- End downloading
*/

/* playlist debug
 ..\..\..\..\Tools\yt-dlp.exe Extracting cookies from firefox
Extracted 65 cookies from firefox
[youtube:tab] Extracting URL: https://www.youtube.com/watch?v=VMmMaOFSKFU&list=PL0TCdRxSHW03sooI3sLlAlp-5Cy92SMqs&pp=gAQB
[youtube:tab] Downloading playlist PL0TCdRxSHW03sooI3sLlAlp-5Cy92SMqs - add --no-playlist to download just the video VMmMaOFSKFU
[youtube:tab] PL0TCdRxSHW03sooI3sLlAlp-5Cy92SMqs: Downloading webpage
[youtube:tab] Extracting URL: https://www.youtube.com/playlist?list=PL0TCdRxSHW03sooI3sLlAlp-5Cy92SMqs
[youtube:tab] PL0TCdRxSHW03sooI3sLlAlp-5Cy92SMqs: Downloading webpage
[youtube:tab] PL0TCdRxSHW03sooI3sLlAlp-5Cy92SMqs: Redownloading playlist API JSON with unavailable videos
[download] Downloading playlist: Music Playlist
[youtube:tab] PL0TCdRxSHW03sooI3sLlAlp-5Cy92SMqs page 1: Downloading API JSON
[youtube:tab] PL0TCdRxSHW03sooI3sLlAlp-5Cy92SMqs page 2: Downloading API JSON
[youtube:tab] Playlist Music Playlist: Downloading 2 items of 2
[download] Downloading item 1 of 2
[youtube] Extracting URL: https://www.youtube.com/watch?v=VMmMaOFSKFU
[youtube] VMmMaOFSKFU: Downloading webpage
[youtube] VMmMaOFSKFU: Downloading tv client config
[youtube] VMmMaOFSKFU: Downloading tv player API JSON
[info] VMmMaOFSKFU: Downloading 1 format(s): 249
[download] Sleeping 6.00 seconds as required by the site...
[download] Destination: C:\UploadedVideos\Tyler Nance - Keeps Me Sane (Official Music Video) [VMmMaOFSKFU].webm

[download]   0.1% of    1.23MiB at    5.53KiB/s ETA 03:46
[download]   0.2% of    1.23MiB at   16.48KiB/s ETA 01:16
[download]   0.6% of    1.23MiB at   38.22KiB/s ETA 00:32
[download]   1.2% of    1.23MiB at   81.89KiB/s ETA 00:15
[download]   2.5% of    1.23MiB at  167.41KiB/s ETA 00:07
[download]   5.0% of    1.23MiB at  174.81KiB/s ETA 00:06
[download]  10.1% of    1.23MiB at  232.90KiB/s ETA 00:04
[download]  20.3% of    1.23MiB at  280.64KiB/s ETA 00:03
[download]  40.7% of    1.23MiB at  464.72KiB/s ETA 00:01
[download]  81.5% of    1.23MiB at  561.06KiB/s ETA 00:00
[download] 100.0% of    1.23MiB at  621.76KiB/s ETA 00:00
[download] 100% of    1.23MiB in 00:00:02 at 484.20KiB/s 
[download] Downloading item 2 of 2
[youtube] Extracting URL: https://www.youtube.com/watch?v=vwXTBQFq_3E
[youtube] vwXTBQFq_3E: Downloading webpage
[youtube] vwXTBQFq_3E: Downloading tv client config
[youtube] vwXTBQFq_3E: Downloading tv player API JSON
[info] vwXTBQFq_3E: Downloading 1 format(s): 249
[download] Sleeping 6.00 seconds as required by the site...
[download] Destination: C:\UploadedVideos\oskar med k - Make Me Feel (Official Video) [vwXTBQFq_3E].webm

[download]   0.1% of    1.20MiB at    5.51KiB/s ETA 03:42
[download]   0.2% of    1.20MiB at   16.43KiB/s ETA 01:14
[download]   0.6% of    1.20MiB at   38.34KiB/s ETA 00:31
[download]   1.2% of    1.20MiB at   81.71KiB/s ETA 00:14
[download]   2.5% of    1.20MiB at  167.96KiB/s ETA 00:07
[download]   5.1% of    1.20MiB at  176.91KiB/s ETA 00:06
[download]  10.4% of    1.20MiB at  227.46KiB/s ETA 00:04
[download]  20.8% of    1.20MiB at  279.62KiB/s ETA 00:03
[download]  41.7% of    1.20MiB at  461.28KiB/s ETA 00:01
[download]  83.4% of    1.20MiB at  614.51KiB/s ETA 00:00
[download] 100.0% of    1.20MiB at  610.88KiB/s ETA 00:00
[download] 100% of    1.20MiB in 00:00:02 at 417.53KiB/s 
[download] Finished downloading playlist: Music Playlist
 */