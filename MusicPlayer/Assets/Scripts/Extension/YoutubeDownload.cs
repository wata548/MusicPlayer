using System;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class YoutubeDownload {

    private static Process _process;
    private const string Unknown = "UnKnown";
    
    public static float Progress { get; private set; } = 0;
    public static float Speed { get; private set; } = 0;
    public static bool Downloading { get; private set; } = false;
    public static string FileName { get; private set; } = Unknown;

    public static int PlayListCount { get; private set; }
    public static int CurPlayListCount { get; private set; }
    
    /// <summary>
    /// It run by async
    /// </summary>
    /// <param name="link">youtube link</param>
    public static void Download(string link) {
        PlayListCount = 0;
        CurPlayListCount = 0;
        Progress = 0f;
        Speed = 0f;
        
        Task.Run(() => DownloadFunc(link));
    }

    public static void TaskKill() {
        
        if (_process != null) {
            _process.Kill();
        }
    }
    
    private static void DownloadFunc(string link) {
        
        Downloading = true;
        PlayListCount = 1;       
        CurPlayListCount = 1;
        FileName = Unknown;

        string ytDlpPath = Path.Combine(Application.streamingAssetsPath , "yt-dlp.exe");
        string outputFolder = Path.Combine(Application.streamingAssetsPath, "Music");
        string arguments = $"--no-progress -x --audio-format mp3 --audio-quality 0 -o \"{outputFolder}/%(title)s.%(ext)s\" \"{link}\"";

        StringBuilder builder = new();
        _process = new Process {
            
            StartInfo = new ProcessStartInfo {
                FileName = ytDlpPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardInputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            }
        };

        //Progress process lamda
        _process.OutputDataReceived += (sender, message) => {

            if (string.IsNullOrEmpty(message.Data))
                return;

            var fileName = Regex.Match(message.Data, @"\[download\] Destination: (.+)");
            var playList = Regex.Match(message.Data, @"Downloading item (\d+) of (\d+)");
            var percent = Regex.Match(message.Data, @"(\d+.\d+)% of ~   (\d+.\d+)MiB");

            if (fileName.Success) {

                FileName = Path.GetFileNameWithoutExtension(fileName.Groups[1].Value);
            }
            if (playList.Success) {
                CurPlayListCount = Convert.ToInt32(playList.Groups[1].Value);
                PlayListCount = Convert.ToInt32(playList.Groups[2].Value);
            }
            if (percent.Success) {

                Progress = Convert.ToSingle(percent.Groups[1].Value);
                Speed = Convert.ToSingle(percent.Groups[2].Value);    
            }
            
        };
        
        _process.Start();
        _process.BeginOutputReadLine();
        _process.WaitForExit();
        _process = null;
        Progress = 1;
        MusicPlayer.Instance.FileUpdate();
        Downloading = false;
    }
}