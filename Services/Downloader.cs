namespace YoTuViLo2.Services;

internal interface ISourceStep
{
    IOptionalStep FromSource(params List<String> source);
}

internal interface IOptionalStep
{
    IOptionalStep AsFormat(DownloadFormat? format);
    IOptionalStep WithScope(DownloadScope? scope);
    Process BuildToProcess();
    IOptionalStep WithAudioExtract(Boolean value);
    IOptionalStep WithMetadata(Boolean value);
    IOptionalStep InDirectory(String directory);
    IOptionalStep OnlyMusic(Boolean value);
    VideoLoaderBuilder BuildEnd();
}


internal class VideoLoaderBuilder:  ISourceStep, IOptionalStep
{
    List<String>? _source { get; set; }
    DownloadFormat? _format { get; set; }
    DownloadScope? _scope { get; set; }
    Boolean _withAudioExtract { get; set; } = false;
    Boolean _withMetadata { get; set; } = false;
    Boolean _onlyMusic { get; set; } = false;
    String _directory { get; set; } = "";

    public IOptionalStep OnlyMusic(Boolean value)
    {
        _onlyMusic = value;
        return this;
    }

    public IOptionalStep InDirectory(String directory)
    {
        _directory = directory;
        return this;
    }

    public IOptionalStep FromSource(params List<String> source)
    {
        if (source.Count == 0) throw new ArgumentException();
        _source = source;
        return this;
    }

    public IOptionalStep AsFormat(DownloadFormat? format)
    {
        _format ??= format;
        return this;
    }

    public IOptionalStep WithScope(DownloadScope? scope)
    {
        _scope ??= scope;
        return this;
    }

    public IOptionalStep WithAudioExtract(Boolean value)
    {
        _withAudioExtract = value;
        return this;
    }

    public IOptionalStep WithMetadata(Boolean value)
    {
        _withMetadata = value;
        return this;
    }

    private ProcessStartInfo BuildPSI()
    {
        ProcessStartInfo psi = new ProcessStartInfo()
        {
            FileName = AppSettings.DownloadToolPath,
            ArgumentList = {
                "-P", _directory == "" ? AppSettings.DownloadPath : _directory,
                "--cookies-from-browser", "firefox",
                "--ffmpeg-location", AppSettings.ConvertToolPath
            },
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            RedirectStandardInput = true
        };

        if (_format is not null)
        {
            psi.ArgumentList.Add("-f");

            String formatLetter = _scope == DownloadScope.Best ? "b" : "w";

            switch (_format)
            {
                case DownloadFormat.Video & DownloadFormat.Audio:
                    {
                        psi.ArgumentList.Add(formatLetter);
                        break;
                    }
                case DownloadFormat.Video:
                    {
                        psi.ArgumentList.Add($"{formatLetter}v");
                        break;
                    }
                case DownloadFormat.Audio:
                    {
                        psi.ArgumentList.Add($"{formatLetter}a");
                        break;
                    }
                default: break;
            }
        }

        if (_withAudioExtract)
        {
            psi.ArgumentList.Add("-x");
            psi.ArgumentList.Add("--audio-format");
            psi.ArgumentList.Add("mp3");
        }

        if (_withMetadata)
        {
            psi.ArgumentList.Add("--embed-metadata");
            psi.ArgumentList.Add("--embed-thumbnail");
        }

        if (_onlyMusic)
        {
            psi.ArgumentList.Add("--match-filters");
            psi.ArgumentList.Add("duration<300");
        }

        if (_source is null) throw new ArgumentNullException();
        foreach (String unit in _source)
        {
            psi.ArgumentList.Add(unit);
        }

        return psi;
    }

    public Process BuildToProcess()
    {
        ProcessStartInfo psi = BuildPSI();
        Process proc = new Process();
        proc.StartInfo = psi;

        return proc;
    }

    public VideoLoaderBuilder BuildEnd()
    {
        return this;
    }

    public String GetCommand()
    {
        ProcessStartInfo psi = BuildPSI();
        return $"{psi.FileName} {psi.Arguments}";
    }

    //void TemporaryMain()
    //{
    //    ProcessStartInfo info = new ProcessStartInfo()
    //    {
    //        FileName = AppSettings.DownloadToolPath,
    //        ArgumentList = {
    //            "-x",
    //            "--audio-format", "best",
    //            "-P", AppSettings.DownloadPath,
    //            "--cookies-from-browser", "firefox",
    //            "--ffmpeg-location", AppSettings.ConvertToolPath,
    //            "https://youtu.be/vOA11hmbOMA"
    //        },
    //        UseShellExecute = false,
    //        CreateNoWindow = true,
    //        RedirectStandardError = true,
    //        RedirectStandardOutput = true,
    //        RedirectStandardInput = true
    //    };

    //    Process c = new Process();
    //    c.StartInfo = info;
    //    //c.OutputDataReceived += OutputDataReceived;
    //    //c.ErrorDataReceived += OutputDataReceived;
    //    //c.Exited += C_Exited;
    //    c.Start();

    //    //Output.Text = "";
    //    //Output.Text += "Start work!\n";
    //    c.BeginOutputReadLine();
    //    c.BeginErrorReadLine();
    //}

    public static VideoLoaderBuilder New()
    {
        return new VideoLoaderBuilder();
    }
}

[Flags]
internal enum DownloadFormat
{
    Video = 1,
    Audio = 2
}

internal enum DownloadScope
{
    Best,
    Worst
}
